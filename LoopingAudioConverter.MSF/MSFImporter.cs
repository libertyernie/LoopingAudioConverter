using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSF {
	public class MSFImporter : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("msf", StringComparison.InvariantCultureIgnoreCase);
		}

		public Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints = null, IProgress<double> progress = null) {
			throw new AudioImporterException("Cannot natively decode this format");
		}

		public IEnumerable<object> TryReadUncompressedAudioFromFile(string filename) {
			byte[] data = File.ReadAllBytes(filename);
			var msf = MSF.Parse(data);
			if (msf is MSF_MP3 x)
				yield return x.MP3;
		}
	}
}
