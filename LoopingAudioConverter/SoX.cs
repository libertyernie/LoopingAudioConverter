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

		public void WriteFile(LWAV lwav, string output_filename) {
			if (output_filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}
			
			byte[] wav = lwav.Export();

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				RedirectStandardInput = true,
				UseShellExecute = false,
				Arguments = "-V1 - \"" + output_filename + "\""
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
