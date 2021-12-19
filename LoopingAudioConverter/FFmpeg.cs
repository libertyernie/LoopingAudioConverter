using RunProcessAsTask;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	/// <summary>
	/// A class to interface with FFmpeg, using it to read and write non-looping audio data and to apply effects.
	/// </summary>
	public class FFmpeg : IAudioImporter, IEffectEngine {
		private readonly string ExePath;

		/// <summary>
		/// Initializes the FFmpeg interfacing class and importer.
		/// </summary>
		/// <param name="exePath">Path to ffmpeg executable (relative or absolute.)</param>
		public FFmpeg(string exePath) {
			ExePath = exePath;
		}

		/// <summary>
		/// Will always return true. FFmpeg supports many file formats, and it should be tried once all importers that support looping audio fail to read a file.
		/// </summary>
		/// <param name="extension">Filename extension</param>
		/// <returns>true</returns>
		public bool SupportsExtension(string extension) {
			return true;
		}

		/// <summary>
		/// Converts a file to WAV using FFmpeg and reads it into a PCM16Audio object.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <returns>A non-looping PCM16Audio</returns>
		public async Task<PCM16Audio> ReadFileAsync(string filename) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("FFmpeg not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string outfile = TempFiles.Create("wav");

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = $"-i \"{filename}\" -f wav -acodec pcm_s16le {outfile}"
			};
			await ProcessEx.RunAsync(psi);

			try {
				PCM16Audio lwav = PCM16Factory.FromFile(outfile, true);
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
		/// <returns>A new PCM16Audio object if one or more effects are applied; the same PCM16Audio object if no effects are applied.</returns>
		public PCM16Audio ApplyEffects(PCM16Audio lwav, int channels = int.MaxValue, decimal db = 0, decimal amplitude = 1, int rate = int.MaxValue, double pitch_semitones = 0, double tempo_ratio = 1) {
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
			if (parameters.Count == 0)
				return lwav;

			string infile = TempFiles.Create("wav");
			string outfile = TempFiles.Create("wav");

			File.WriteAllBytes(infile, wav);

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = $"-i {infile} {string.Join(" ", parameters)} -f wav -c:a pcm_s16le {outfile}"
			};
			Process p = Process.Start(psi);
			p.WaitForExit();
			File.Delete(infile);

			try {
				PCM16Audio l = PCM16Factory.FromFile(outfile, true);
				l.Looping = lwav.Looping;
				l.LoopStart = lwav.LoopStart;
				l.LoopEnd = lwav.LoopEnd;

				if (l.Looping && l.SampleRate != lwav.SampleRate) {
					// When the sample rate is changed, we need to change the loop points to match.
					double ratio = (double)l.SampleRate / lwav.SampleRate;
					l.LoopStart = (int)(l.LoopStart * ratio);
					l.LoopEnd = (int)(l.LoopEnd * ratio);
				}

				if (l.Looping && tempo_ratio != 1) {
					// When the tempo is changed, we need to change the loop points to match.
					l.LoopStart = (int)(l.LoopStart / tempo_ratio);
					l.LoopEnd = (int)(l.LoopEnd / tempo_ratio);
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
		public async Task WriteFileAsync(PCM16Audio lwav, string output_filename, string encodingParameters) {
			if (output_filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string infile = TempFiles.Create("wav");
			File.WriteAllBytes(infile, lwav.Export());

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				Arguments = $"-i {infile} {encodingParameters} \"{output_filename}\"",
				UseShellExecute = false,
				CreateNoWindow = true
			};
			var pr = await ProcessEx.RunAsync(psi);
			File.Delete(infile);

			if (pr.ExitCode != 0) {
				throw new AudioExporterException("ffmpeg quit with exit code " + pr.ExitCode);
			}
		}

		public string GetImporterName() {
			return "FFmpeg";
		}

		PCM16Audio IEffectEngine.ApplyEffects(PCM16Audio lwav, int channels, decimal db, decimal amplitude, int rate, decimal pitch_semitones, decimal tempo_ratio) {
			return ApplyEffects(lwav, channels, db, amplitude, rate, (double)pitch_semitones, (double)tempo_ratio);
		}
	}
}
