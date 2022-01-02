using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisImporter : IOpinionatedAudioImporter {
		private readonly FFmpegEngine effectEngine;

		public VorbisImporter(FFmpegEngine effectEngine) {
			this.effectEngine = effectEngine;
		}

		public bool SupportsExtension(string extension) {
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            return new[] { "ogg", "logg", "oga" }.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool SharesCodecsWith(IAudioExporter exporter) => exporter is VorbisExporter;

        public async Task<PCM16Audio> ReadFileAsync(string filename, IProgress<double> progress) {
            var audio1 = await effectEngine.ReadFileAsync(filename, progress: progress);

			var originalFile = File.ReadAllBytes(filename);
			return new VorbisAudio(originalFile, audio1);
		}
    }
}
