using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisImporter : IAudioImporter {
		private readonly FFmpegEngine effectEngine;

		public VorbisImporter(FFmpegEngine effectEngine) {
			this.effectEngine = effectEngine;
		}

		public bool SupportsExtension(string extension) {
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            return new[] { "ogg", "logg", "oga" }.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
        }

		public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
            var pcm = await effectEngine.ReadFileAsync(filename, hints, progress);

			using (VorbisFile vorbisFile = new VorbisFile(File.ReadAllBytes(filename))) {
				VorbisComments c = vorbisFile.GetPageHeaders()
					.Select(p => p.GetCommentHeader())
					.Where(h => h != null)
					.Select(h => h.ExtractComments())
					.DefaultIfEmpty(new VorbisComments())
					.First();
				if (c.Comments.TryGetValue("LOOPSTART", out string loopStart)) {
					pcm.Looping = true;
					pcm.LoopStart = int.Parse(loopStart);
				}
				if (c.Comments.TryGetValue("LOOPLENGTH", out string loopLength)) {
					pcm.LoopEnd = int.Parse(loopStart) + int.Parse(loopLength);
				}
			}

			return pcm;
		}

		public IEnumerable<object> TryReadFile(string filename) {
			var originalFile = File.ReadAllBytes(filename);
			yield return new VorbisAudio(originalFile);
		}
	}
}
