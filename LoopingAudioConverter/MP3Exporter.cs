using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MP3Exporter : IAudioExporter {
		private string ExePath;

		public MP3Exporter(string exePath) {
            ExePath = exePath;
        }

		public void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext) {
			string outPath = Path.Combine(output_dir, original_filename_no_ext + ".mp3");
			if (outPath.Contains("\"")) {
				throw new AudioExporterException("Invalid character (\") found in output filename");
			}

			byte[] wav = lwav.Export();

				ProcessStartInfo psi = new ProcessStartInfo {
					FileName = ExePath,
					RedirectStandardInput = true,
					UseShellExecute = false,
					Arguments = "- \"" + outPath + "\""
				};
				Process p = Process.Start(psi);
				p.StandardInput.BaseStream.WriteAsync(wav, 0, wav.Length).ContinueWith(t => p.StandardInput.BaseStream.Close());
				p.WaitForExit();
				if (p.ExitCode != 0) {
					throw new AudioExporterException("LAME quit with exit code " + p.ExitCode);
				}
		}

		public string GetExporterName() {
			return "LAME";
		}
	}
}
