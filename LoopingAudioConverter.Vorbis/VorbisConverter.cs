using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisConverter : IAudioImporter, IAudioExporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;

		public VorbisConverter(FFmpegEngine effectEngine, string encoding_parameters) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
		}

		public bool SupportsExtension(string extension) {
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            return new[] { "ogg", "logg", "oga" }.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<PCM16Audio> ReadFileAsync(string filename) {
            var audio1 = await effectEngine.ReadFileAsync(filename);

			var originalFile = File.ReadAllBytes(filename);
			return new VorbisAudio(originalFile, audio1);
		}

        public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");

			VorbisAudio audio;
			if (lwav is VorbisAudio v) {
				audio = v;
			} else {
				string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".ogg");
				await effectEngine.WriteFileAsync(lwav, tempFile, encoding_parameters);
				audio = new VorbisAudio(File.ReadAllBytes(tempFile), lwav) {
                    LoopStart = lwav.LoopStart,
                    LoopEnd = lwav.LoopEnd,
                    Looping = lwav.Looping
                };
				File.Delete(tempFile);
			}

			File.WriteAllBytes(output_filename, audio.Export());
		}
    }
}
