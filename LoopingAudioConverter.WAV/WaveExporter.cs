using LoopingAudioConverter.PCM;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.WAV {
	public class WaveExporter : IAudioExporter {
		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".wav");
			File.WriteAllBytes(output_filename, lwav.Export());
			return Task.CompletedTask;
		}
	}
}
