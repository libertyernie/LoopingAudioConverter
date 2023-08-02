using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisImporter : IAudioImporter {
		public bool SupportsExtension(string extension) {
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            return new[] { "ogg", "logg", "oga" }.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
		}

		public Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints = null, IProgress<double> progress = null) {
			throw new AudioImporterException("Cannot natively decode this format");
		}

		public IEnumerable<object> TryReadUncompressedAudioFromFile(string filename) {
			var originalFile = File.ReadAllBytes(filename);
			yield return new VorbisAudio(originalFile);
		}
	}
}
