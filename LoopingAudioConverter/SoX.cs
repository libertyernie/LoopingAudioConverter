using RSTMLib.WAV;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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
		/// Converts a file to WAV using SoX and reads it into an LWAV object.
		/// If the format is not supported, SoX will write a message to the console and this function will throw an AudioImporterException.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <returns>A non-looping LWAV</returns>
        public PCM16Audio ReadFile(string filename) {
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
            Process p = Process.Start(psi);
            p.WaitForExit();

            try {
                PCM16Audio lwav = PCM16Factory.FromFile(outfile, true);
                lwav.OriginalFilePath = filename;
                return lwav;
            } catch (Exception e) {
                throw new AudioImporterException("Could not read SoX output: " + e.Message);
            }
        }

		/// <summary>
		/// Applies one or more SoX effects to the LWAV given and reads the result into a new LWAV.
		/// Intended to either adjust the volume of the audio or reduce the file size.
		/// </summary>
		/// <param name="lwav">The LWAV to use as an input</param>
		/// <param name="max_channels">The new number of channels (if the LWAV already has this number of channels or fewer, this effect will not be applied)</param>
		/// <param name="db">Volume adjustment, in decibels (if 0, this effect will not be applied)</param>
		/// <param name="amplitude">Volume adjustment, in linear ratio (if 1, this effect will not be applied)</param>
		/// <param name="max_rate">The new sample rate (if the LWAV's sample rate is less than or equal to this value, this effect will not be applied)</param>
		/// <returns>A new LWAV object if one or more effects are applied; the same LWAV object if no effects are applied.</returns>
		public PCM16Audio ApplyEffects(PCM16Audio lwav, int max_channels = int.MaxValue, decimal db = 0, decimal amplitude = 1, int max_rate = int.MaxValue) {
			byte[] wav = lwav.Export();

			int channels = Math.Min(max_channels, lwav.Channels);
			int sampleRate = Math.Min(max_rate, lwav.SampleRate);

			StringBuilder effects_string = new StringBuilder();
			if (channels != lwav.Channels) {
				effects_string.Append(" channels " + max_channels);
			}
			if (db != 0) {
				effects_string.Append(" vol " + db + " dB");
			}
			if (amplitude != 1) {
				effects_string.Append(" vol " + amplitude + " amplitude");
			}
			if (sampleRate != lwav.SampleRate) {
				effects_string.Append(" rate " + max_rate);
			}

			if (effects_string.Length == 0) {
				// No effects will be performed - just return the same LWAV that was passed in without calling SoX unnecessarily
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
				return l;
			} catch (Exception e) {
				throw new AudioImporterException("Could not read SoX output: " + e.Message);
			}
		}

		/// <summary>
		/// Writes the LWAV to the given file, using SoX to make sure the format is correct. SoX can encode and write FLAC and Ogg Vorbis files, among others.
		/// </summary>
		/// <param name="lwav">Input audio</param>
		/// <param name="output_filename">Path of output file</param>
		public void WriteFile(PCM16Audio lwav, string output_filename, string encodingParameters = null) {
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
			Process p = Process.Start(psi);
			p.WaitForExit();

            File.Delete(infile);

			if (p.ExitCode != 0) {
				throw new AudioExporterException("SoX quit with exit code " + p.ExitCode);
			}
		}

        public string GetImporterName() {
            return "SoX";
        }
    }
}
