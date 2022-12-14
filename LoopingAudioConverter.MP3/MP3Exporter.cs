using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MP3 {
	public class MP3Exporter : IAudioExporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;

		public MP3Exporter(FFmpegEngine effectEngine, string encoding_parameters) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
		}

		public bool TryWriteCompressedAudioToFile(object audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext) {
			if (audio is MP3Audio mp3 && !loopPoints.Looping) {
				string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".mp3");
				File.WriteAllBytes(output_filename, mp3.Data);
				return true;
			}
			return false;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".mp3");
			await effectEngine.WriteFileAsync(lwav, output_filename, encoding_parameters, progress);
		}
	}
}
