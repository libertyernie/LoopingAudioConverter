using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoopingAudioConverter.MSF {
	public class MSFImporter : ICompressedAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("msf", StringComparison.InvariantCultureIgnoreCase);
		}

		public IEnumerable<object> TryReadFile(string filename) {
			byte[] data = File.ReadAllBytes(filename);
			var msf = MSF.Parse(data);
			if (msf is MSF_MP3 x)
				yield return x.MP3;
		}
	}
}
