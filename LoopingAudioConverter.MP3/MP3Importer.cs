using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoopingAudioConverter.MP3 {
	public class MP3Importer : ICompressedAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("mp3", StringComparison.InvariantCultureIgnoreCase);
		}

		public IEnumerable<object> TryReadFile(string filename) {
			byte[] mp3data = File.ReadAllBytes(filename);
			yield return new MP3Audio(mp3data);
		}
	}
}
