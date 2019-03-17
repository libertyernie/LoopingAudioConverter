using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class WAVExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".wav");
			File.WriteAllBytes(output_filename, lwav.Export());
			return Task.FromResult(0);
		}

		public string GetExporterName() {
			return "WAVExporter";
		}
	}
}
