﻿using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using RunProcessAsTask;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.FFmpeg {
	/// <summary>
	/// A class to interface with FFmpeg, using it to read and write non-looping audio data and to apply effects.
	/// </summary>
	public class FFmpegEngine : IAudioImporter {
		private readonly string ExePath;

		/// <summary>
		/// Initializes the FFmpeg interfacing class and importer.
		/// </summary>
		/// <param name="exePath">Path to ffmpeg executable (relative or absolute.)</param>
		public FFmpegEngine(string exePath) {
			ExePath = exePath;
		}

		bool IAudioImporter.SupportsExtension(string extension) => true;

		bool IAudioImporter.SharesCodecsWith(IAudioExporter exporter) => false;

		private struct Metadata {
			public string codec;
			public TimeSpan? duration;
        }

		private async Task<Metadata> GetInputMetadataAsync(string filename) {
			Metadata m = new Metadata {
				codec = null,
				duration = null
			};
			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = $"-t 1.0 -i \"{filename}\" -f null -"
			};
			var result = await ProcessEx.RunAsync(psi);
			foreach (string line in result.StandardError) {
				if (line.StartsWith("Input #0, ")) {
					m.codec = line.Substring("Input #0, ".Length).Split(',').First();
                } else if (line.StartsWith("  Duration: ")) {
					string str = line.Substring("  Duration: ".Length).Split(',').First();
					if (TimeSpan.TryParse(str, out TimeSpan ts))
						m.duration = ts;
				}
			}
			return m;
		}

		/// <summary>
		/// Converts a file to WAV using FFmpeg and reads it into a PCM16Audio object.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <param name="input_format_parameters">Additional parameters for ffmpeg to include just before the input parameter (optional)</param>
		/// <param name="default_max_duration">A maximum duration for the input file. Only used if the actual duration cannot be determined.</param>
		/// <param name="progress">Progress bar callback (values range from 0.0 to 1.0, inclusive) (optional)</param>
		/// <returns>A non-looping PCM16Audio</returns>
		public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress = null) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("FFmpeg not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			var metadata = await GetInputMetadataAsync(filename);

			TimeSpan? actual_duration = metadata.duration;
			TimeSpan expectedDuration = hints.Duration ?? actual_duration ?? TimeSpan.FromMinutes(10);

			string outfile = Path.GetTempFileName();

			IEnumerable<string> getArgs() {
				yield return "-y";
				if (metadata.codec == "libgme") {
					yield return "-f libgme";
					if (hints.RenderingSampleRate is int gr) {
						yield return $"-sample_rate {gr}";
                    }
				}
				if (actual_duration == null) {
					yield return $"-t {expectedDuration.TotalSeconds + 1}";
				}
				yield return $"-i \"{filename}\"";
				yield return "-f wav";
				yield return "-acodec pcm_s16le";
				yield return $"\"{outfile}\"";
				yield return "-progress pipe:1";
			}
			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				Arguments = string.Join(" ", getArgs())
			};
			var process = Process.Start(psi);
			using (var sr = process.StandardOutput) {
				double expected_ticks = expectedDuration.Ticks;

				string line;
				while ((line = await sr.ReadLineAsync()) != null) {
					foreach (var str in line.Split(' ')) {
						if (str.StartsWith("out_time=") && TimeSpan.TryParse(str.Substring(9), out TimeSpan ts)) {
							double ratio = ts.Ticks / expected_ticks;
							progress?.Report(Math.Min(1.0, ratio));
                        }
					}
				}
            }
			process.WaitForExit();

			try {
				PCM16Audio lwav = WaveConverter.FromFile(outfile, true);
				return lwav;
			} catch (Exception e) {
				throw new AudioImporterException("Could not read ffmpeg output: " + e.Message);
			}
		}

		/// <summary>
		/// Applies one or more effects to the PCM16Audio given and reads the result into a new PCM16Audio.
		/// Intended to either adjust the volume of the audio or reduce the file size.
		/// </summary>
		/// <param name="lwav">The PCM16Audio to use as an input</param>
		/// <param name="channels">The new number of channels (if the PCM16Audio already has this number of channels, this effect will not be applied)</param>
		/// <param name="db">Volume adjustment, in decibels (if 0, this effect will not be applied)</param>
		/// <param name="amplitude">Volume adjustment, in linear ratio (if 1, this effect will not be applied)</param>
		/// <param name="rate">The new sample rate (if the PCM16Audio's sample rate is equal to this value, this effect will not be applied)</param>
		/// <param name="pitch_semitones">Pitch adjustment, in semitones (if 0, this effect will not be applied)</param>
		/// <param name="tempo_ratio">Tempo ratio (if 1, this effect will not be applied)</param>
		/// <param name="force">If true, always return a new PCM16Audio object</param>
		/// <returns>A new PCM16Audio object if one or more effects are applied; the same PCM16Audio object if no effects are applied.</returns>
		public PCM16Audio ApplyEffects(PCM16Audio lwav, int channels = int.MaxValue, decimal db = 0, decimal amplitude = 1, int rate = int.MaxValue, double pitch_semitones = 0, double tempo_ratio = 1, bool force = false) {
			byte[] wav = lwav.Export();

			// This is where I wish I had F# list comprehensions

			IEnumerable<string> getParameters() {
				if (channels != lwav.Channels)
					yield return $"-ac {channels}";

				IEnumerable<string> getFilters() {
					if (db != 0)
						yield return $"volume={db.ToString(CultureInfo.InvariantCulture)}dB";
					if (amplitude != 1)
						yield return $"volume={amplitude.ToString(CultureInfo.InvariantCulture)}";

					double n = pitch_semitones;
					double r = lwav.SampleRate;
					double t = Math.Pow(2, n / 12.0);
					double tempo = tempo_ratio / t;
					double newrate = t * r;
					if (newrate != lwav.SampleRate) {
						yield return $"asetrate={newrate}";
					}
					while (tempo > 2) {
						yield return $"atempo={tempo}";
						tempo /= 2;
					}
					if (tempo != 1) {
						yield return $"atempo={tempo}";
					}
					if (newrate > rate) {
						yield return $"aresample={rate}";
					}
				}

				var filters = getFilters().ToList();
				if (filters.Count > 0)
					yield return $"-af \"{string.Join(", ", filters)}\"";
			}

			var parameters = getParameters().ToList();
			if (parameters.Count == 0 && !force)
				return lwav;

			string infile = Path.GetTempFileName();
			string outfile = Path.GetTempFileName();

			File.WriteAllBytes(infile, wav);

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-y -f wav -i {infile} {string.Join(" ", parameters)} -f wav -c:a pcm_s16le {outfile}"
			};
			Process p = Process.Start(psi);
			p.WaitForExit();
			File.Delete(infile);

			try {
				PCM16Audio l = WaveConverter.FromFile(outfile, true);
				l.Loop = lwav.Loop;

				if (l.Looping && l.SampleRate != lwav.SampleRate) {
					// When the sample rate is changed, we need to change the loop points to match.
					double ratio = (double)l.SampleRate / lwav.SampleRate;
					l.Loop = Immutable.LoopType.NewLooping(
						(int)(l.LoopStart * ratio),
						(int)(l.LoopEnd * ratio));
				}

				if (l.Looping && tempo_ratio != 1) {
					// When the tempo is changed, we need to change the loop points to match.
					l.Loop = Immutable.LoopType.NewLooping(
						(int)(l.LoopStart / tempo_ratio),
						(int)(l.LoopEnd / tempo_ratio));
				}
				return l;
			} catch (Exception e) {
				throw new AudioImporterException("Could not read ffmpeg output: " + e.Message);
			}
		}

		/// <summary>
		/// Writes the PCM16Audio to the given file, using ffmpeg to make sure the format is correct.
		/// </summary>
		/// <param name="lwav">Input audio</param>
		/// <param name="output_filename">Path of output file</param>
		/// <param name="encodingParameters">Additional encoding parameters for ffmpeg</param>
		/// <param name="progress">Progress tracker (optional)</param>
		public async Task WriteFileAsync(PCM16Audio lwav, string output_filename, string encodingParameters, IProgress<double> progress = null) {
			if (output_filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string infile = Path.GetTempFileName();
			File.WriteAllBytes(infile, lwav.Export());

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				Arguments = $"-y -f wav -i {infile} {encodingParameters} \"{output_filename}\" -progress pipe:1",
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true
			};
			var process = Process.Start(psi);
			using (var sr = process.StandardOutput) {
				double sample_count = lwav.Samples.Length / lwav.Channels;
				double seconds_count = sample_count / lwav.SampleRate;
				double expected_ticks = TimeSpan.FromSeconds(seconds_count).Ticks;

				string line;
				while ((line = await sr.ReadLineAsync()) != null) {
					foreach (var str in line.Split(' ')) {
						if (str.StartsWith("out_time=") && TimeSpan.TryParse(str.Substring(9), out TimeSpan ts)) {
							double ratio = ts.Ticks / expected_ticks;
							progress?.Report(Math.Min(1.0, ratio));
						}
					}
				}
			}
			process.WaitForExit();
			File.Delete(infile);

			if (process.ExitCode != 0) {
				throw new AudioExporterException("ffmpeg quit with exit code " + process.ExitCode);
			}
		}
	}
}
