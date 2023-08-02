using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.WAV {
	public class WaveExporter : IAudioExporter {
		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".wav");
			File.WriteAllBytes(output_filename, lwav.Export());
			return Task.CompletedTask;
		}

		public bool TryWriteCompressedAudioToFile(object audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext) {
			return false;
		}
	}
}
