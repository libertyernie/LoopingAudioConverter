using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LoopingAudioConverter {
	public class MP3Importer : IAudioImporter {
		private string ExePath;

		public MP3Importer(string exePath) {
			ExePath = exePath;
		}

		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("mp3", StringComparison.InvariantCultureIgnoreCase);
		}

		public PCM16Audio ReadFile(string filename) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("madplay not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			string outfile = TempFiles.Create("wav");

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "-v -o wav:" + outfile + " \"" + filename + "\""
			};
			Process p = Process.Start(psi);
			p.WaitForExit();

			try {
				return PCM16Factory.FromFile(outfile, true);
			} catch (PCM16FactoryException e) {
				throw new AudioImporterException("Could not read madplay output: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "madplay";
		}
	}
}
