using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MP3 {
	public class MP3Importer : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("mp3", StringComparison.InvariantCultureIgnoreCase);
		}

		public Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints = null, IProgress<double> progress = null) {
			throw new AudioImporterException("Cannot natively decode this format");
		}

		public IEnumerable<object> TryReadUncompressedAudioFromFile(string filename) {
			byte[] mp3data = File.ReadAllBytes(filename);
			yield return new MP3Audio(mp3data);
		}
	}
}
