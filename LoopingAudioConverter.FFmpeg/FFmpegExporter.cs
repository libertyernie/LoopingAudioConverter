using LoopingAudioConverter.MP3;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.FFmpeg {
	public class FFmpegExporter : IAudioExporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;
		private readonly string output_extension;

		public FFmpegExporter(FFmpegEngine effectEngine, string encoding_parameters, string output_extension) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
			this.output_extension = output_extension;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + output_extension);

			if (lwav is MP3Audio mp3 && output_extension == ".mp3") {
				File.WriteAllBytes(output_filename, mp3.MP3Data);
				return;
			}

			await effectEngine.WriteFileAsync(lwav, output_filename, encoding_parameters);
		}
	}
}
