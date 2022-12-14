using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisImporter : ICompressedAudioImporter {
		private readonly FFmpegEngine effectEngine;

		public VorbisImporter(FFmpegEngine effectEngine) {
			this.effectEngine = effectEngine;
		}

		public bool SupportsExtension(string extension) {
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            return new[] { "ogg", "logg", "oga" }.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
        }

		public IEnumerable<object> TryReadFile(string filename) {
			var originalFile = File.ReadAllBytes(filename);
			yield return new VorbisAudio(originalFile);
		}
	}
}
