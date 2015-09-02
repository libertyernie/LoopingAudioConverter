using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class LWAVImporter : IAudioImporter {
		private static string[] EXTENSIONS = new string[] { "wav", "lwav" };

		public bool SupportsExtension(string extension) {
			while (extension.StartsWith(".")) extension = extension.Substring(1);
			return EXTENSIONS.Any(s => s.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
		}

		public LWAV ReadFile(string filename) {
			try {
				return LWAVFactory.FromByteArray(File.ReadAllBytes(filename));
			} catch (WaveDataException e) {
				throw new AudioImporterException(e.Message, e);
			}
		}

		public string GetImporterName() {
			return "LWAVImporter";
		}
	}
}
