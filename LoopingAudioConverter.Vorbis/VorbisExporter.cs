using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisExporter : IAudioExporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;

		public VorbisExporter(FFmpegEngine effectEngine, string encoding_parameters) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
		}

        public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");

			VorbisAudio audio;
			if (lwav is VorbisAudio v) {
				// Data is already encoded and loop points are already set
				audio = v;
			} else {
				// Encode to temporary file
				string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".ogg");
				await effectEngine.WriteFileAsync(lwav, tempFile, encoding_parameters, progress);
				// Read temporary file
				audio = VorbisAudio.Create(File.ReadAllBytes(tempFile), lwav.Audio);
				// Apply original loop points
				audio.Loop = lwav.Loop;
				// Delete temporary file
				File.Delete(tempFile);
			}

			File.WriteAllBytes(output_filename, audio.Export());
		}
    }
}
