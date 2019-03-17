using RunProcessAsTask;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MP4Importer : IAudioImporter {
		private string ExePath;

		public MP4Importer(string exePath) {
			ExePath = exePath;
		}

		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			foreach (string ext in new string[] { "m4a", "mp4", "aac" }) {
				if (extension.Equals(ext, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			return false;
		}

		public async Task<PCM16Audio> ReadFileAsync(string filename) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("faad not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string outfile = TempFiles.Create("wav");

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "-o " + outfile + " \"" + filename + "\""
			};
			var pr = await ProcessEx.RunAsync(psi);

			try {
				return PCM16Factory.FromFile(outfile, true);
			} catch (PCM16FactoryException e) {
				throw new AudioImporterException("Could not read faad output: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "FAAD";
		}
	}
}
