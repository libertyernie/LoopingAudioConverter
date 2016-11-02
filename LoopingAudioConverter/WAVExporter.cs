using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class WAVExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".wav");
			File.WriteAllBytes(output_filename, lwav.Export());
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "LWAVExporter";
		}
	}
}
