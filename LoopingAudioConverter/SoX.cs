using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
    public class SoX : IAudioImporter {
        private string ExePath;

        public SoX(string exePath) {
            ExePath = exePath;
        }

        public bool SupportsExtension(string extension) {
            return true;
        }

        public LWAV ReadFile(string filename) {
            if (!File.Exists(ExePath)) {
                throw new AudioImporterException("test.exe not found at path: " + ExePath);
            }
            if (filename.Contains('"')) {
                throw new AudioImporterException("File paths with double quote marks (\") are not supported");
            }

            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = ExePath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = "\"" + filename + "\" -b 16 -t wav -"
            };
            Process p = Process.Start(psi);

            try {
                return LWAVFactory.FromStream(p.StandardOutput.BaseStream);
            } catch (Exception e) {
                throw new AudioImporterException("Could not read SoX output: " + e.Message);
            }
        }

		public LWAV ApplyEffects(LWAV lwav, int? channels, double? db, double? amplitude, int? rate) {
			byte[] wav = lwav.Export();

			StringBuilder effects_string = new StringBuilder();
			if (channels != null) {
				effects_string.Append(" channels " + channels);
			}
			if (db != null) {
				effects_string.Append(" vol " + db + " dB");
			}
			if (amplitude != null) {
				effects_string.Append(" vol " + amplitude + " amplitude");
			}
			if (rate != null) {
				effects_string.Append(" rate " + rate);
			}

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				Arguments = @"-t wav - -t wav -" + effects_string
			};
			Process p = Process.Start(psi);
			new Task(() => {
				p.StandardInput.BaseStream.Write(wav, 0, wav.Length);
				p.StandardInput.BaseStream.Close();
			}).Start();

			try {
				LWAV l = LWAVFactory.FromStream(p.StandardOutput.BaseStream, true);
				l.Looping = lwav.Looping;
				l.LoopStart = lwav.LoopStart;
				l.LoopEnd = lwav.LoopEnd;

				if (l.Looping && l.SampleRate != lwav.SampleRate) {
					double ratio = (double)l.SampleRate / lwav.SampleRate;
					l.LoopStart = (int)(l.LoopStart * ratio);
					l.LoopEnd = (int)(l.LoopEnd * ratio);
				}
				return l;
			} catch (Exception e) {
				throw new AudioImporterException("Could not read SoX output: " + e.Message);
			}
		}

		public void WriteFile(LWAV lwav, string output_filename) {
			if (output_filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}
			
			byte[] wav = lwav.Export();

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				RedirectStandardInput = true,
				UseShellExecute = false,
				Arguments = "- \"" + output_filename + "\""
			};
			Process p = Process.Start(psi);
			p.StandardInput.BaseStream.Write(wav, 0, wav.Length);
			p.StandardInput.BaseStream.Close();
			p.WaitForExit();

			if (p.ExitCode != 0) {
				throw new AudioExporterException("SoX quit with exit code " + p.ExitCode);
			}
		}

        public string GetImporterName() {
            return "SoX";
        }
    }
}
