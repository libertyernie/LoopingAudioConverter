using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
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

        public bool SharesCodecsWith(IAudioExporter exporter) => exporter is VorbisExporter;

		bool IAudioImporter.SharesCodecsWith(IAudioExporter exporter) => exporter is VorbisExporter;

		public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
            var audio1 = await effectEngine.ReadFileAsync(filename, hints, progress);

			var originalFile = File.ReadAllBytes(filename);
			return VorbisAudio.Create(originalFile, audio1.ImmutableAudio);
		}
    }
}
