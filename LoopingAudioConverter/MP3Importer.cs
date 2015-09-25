using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
				throw new AudioImporterException("test.exe not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			byte[] data = File.ReadAllBytes(filename);

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				Arguments = "-v -o wav:- \"" + filename + "\""
			};
			Process p = Process.Start(psi);

			try {
				return PCM16Factory.FromStream(p.StandardOutput.BaseStream);
			} catch (PCM16FactoryException e) {
				throw new AudioImporterException("Could not read madplay output: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "madplay";
		}
	}
}
