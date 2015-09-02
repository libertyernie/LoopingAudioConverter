using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class VGMStreamImporter : IAudioImporter {
		private string TestExePath;

		public VGMStreamImporter(string test_exe_path) {
			TestExePath = test_exe_path;
		}

		public bool SupportsExtension(string extension) {
			return true;
		}

		public LWAV ReadFile(string filename) {
			if (!File.Exists(TestExePath)) {
				throw new AudioImporterException("test.exe not found at path: " + TestExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = TestExePath,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				Arguments = "-L -p -l 2 \"" + filename + "\""
			};
			Process p = Process.Start(psi);
			try {
				return LWAVFactory.FromStream(p.StandardOutput.BaseStream);
			} catch (Exception e) {
				throw new AudioImporterException("Could not read output of test.exe: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "VGMStreamImporter";
		}
	}
}
