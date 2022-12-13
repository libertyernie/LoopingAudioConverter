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

		public void TryWriteFile(IAudio audio, string output_dir, string original_filename_no_ext) {
			if (audio is VorbisAudio v) {
				string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");
				File.WriteAllBytes(output_filename, v.Export());
			}
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");

			string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".ogg");
			await effectEngine.WriteFileAsync(lwav, tempFile, encoding_parameters, progress);
			VorbisAudio audio = new VorbisAudio(File.ReadAllBytes(tempFile), effectEngine) {
                LoopStart = lwav.LoopStart,
                LoopEnd = lwav.LoopEnd,
                Looping = lwav.Looping
            };
			File.Delete(tempFile);

			File.WriteAllBytes(output_filename, audio.Export());
		}
    }
}
