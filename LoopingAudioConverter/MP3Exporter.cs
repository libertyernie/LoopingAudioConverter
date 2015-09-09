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

		public void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			string outPath = Path.Combine(output_dir, original_filename_no_ext + ".mp3");
			if (outPath.Contains("\"")) {
				throw new AudioExporterException("Invalid character (\") found in output filename");
			}

			byte[] wav = lwav.Export();

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				RedirectStandardInput = true,
				UseShellExecute = false,
				Arguments = "--silent - \"" + outPath + "\""
			};
			Process p = Process.Start(psi);
			p.StandardInput.BaseStream.Write(wav, 0, wav.Length);
			p.StandardInput.BaseStream.Close();
			p.WaitForExit();
			if (p.ExitCode != 0) {
				throw new AudioExporterException("LAME quit with exit code " + p.ExitCode);
			}
		}

		public Task WriteFileAsync(LWAV lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "MP3 (LAME)";
		}
	}
}
