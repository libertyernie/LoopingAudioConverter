using RunProcessAsTask;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	/// <summary>
	/// A class to interface with SoX, using it to read and write non-looping audio data and to apply effects.
	/// </summary>
	public class SoX : IAudioImporter {
		private string ExePath;

		/// <summary>
		/// Initializes the SoX interfacing class and importer.
		/// </summary>
		/// <param name="exePath">Path to SoX executable (relative or absolute.)</param>
		public SoX(string exePath) {
			ExePath = exePath;
		}

		/// <summary>
		/// Will always return true. SoX supports many file formats, and it should be tried once all importers that support looping audio fail to read a file.
		/// </summary>
		/// <param name="extension">Filename extension</param>
		/// <returns>true</returns>
		public bool SupportsExtension(string extension) {
			return true;
		}

		/// <summary>
		/// Converts a file to WAV using SoX and reads it into a PCM16Audio object.
		/// If the format is not supported, SoX will write a message to the console and this function will throw an AudioImporterException.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <returns>A non-looping PCM16Audio</returns>
		public async Task<PCM16Audio> ReadFileAsync(string filename) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("test.exe not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string outfile = TempFiles.Create("wav");

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "\"" + filename + "\" -b 16 -t wav " + outfile
			};
			var pr = await ProcessEx.RunAsync(psi);

			try {
				PCM16Audio lwav = PCM16Factory.FromFile(outfile, true);
				return lwav;
			} catch (Exception e) {
				throw new AudioImporterException("Could not read SoX output: " + e.Message);
			}
		}

		/// <summary>
		/// Applies one or more SoX effects to the PCM16Audio given and reads the result into a new PCM16Audio.
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
		public PCM16Audio ApplyEffects(PCM16Audio lwav, int channels = int.MaxValue, decimal db = 0, decimal amplitude = 1, int rate = int.MaxValue, decimal pitch_semitones = 0, decimal tempo_ratio = 1) {
			byte[] wav = lwav.Export();

			StringBuilder effects_string = new StringBuilder();
			if (channels != lwav.Channels) {
				effects_string.Append(" channels " + channels);
			}
			if (db != 0) {
				effects_string.Append(" vol " + db.ToString(CultureInfo.InvariantCulture) + " dB");
			}
			if (amplitude != 1) {
				effects_string.Append(" vol " + amplitude.ToString(CultureInfo.InvariantCulture) + " amplitude");
			}
			if (pitch_semitones != 0) {
				effects_string.Append($" pitch {(int)(pitch_semitones * 100)}");
			}
			if (tempo_ratio != 1) {
				effects_string.Append($" tempo {tempo_ratio}");
			}
			if (rate != lwav.SampleRate) {
				effects_string.Append(" rate " + rate);
			}

			if (effects_string.Length == 0) {
				// No effects will be performed - just return the same PCM16Audio that was passed in without calling SoX unnecessarily
				return lwav;
			}

			string infile = TempFiles.Create("wav");
			string outfile = TempFiles.Create("wav");

			File.WriteAllBytes(infile, wav);

			// Sometimes when SoX changes sample rate and sends the result to stdout, it gives the wrong length in the data chunk. Let's just have it send us raw PCM data instead.
			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "-t wav " + infile + " -t wav " + outfile + " " + effects_string
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
				throw new AudioImporterException("Could not read SoX output: " + e.Message);
			}
		}

		/// <summary>
		/// Writes the PCM16Audio to the given file, using SoX to make sure the format is correct. SoX can encode and write FLAC and Ogg Vorbis files, among others.
		/// </summary>
		/// <param name="lwav">Input audio</param>
		/// <param name="output_filename">Path of output file</param>
		public async Task WriteFileAsync(PCM16Audio lwav, string output_filename, string encodingParameters = null) {
			if (output_filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string infile = TempFiles.Create("wav");
			File.WriteAllBytes(infile, lwav.Export());

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				Arguments = infile + " " + (encodingParameters ?? "") + " \"" + output_filename + "\"",
				UseShellExecute = false,
				CreateNoWindow = true
			};
			var pr = await ProcessEx.RunAsync(psi);
			File.Delete(infile);

			if (pr.ExitCode != 0) {
				throw new AudioExporterException("SoX quit with exit code " + pr.ExitCode);
			}
		}

		public string GetImporterName() {
			return "SoX";
		}
	}
}
