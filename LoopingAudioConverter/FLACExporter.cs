using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class FLACExporter : IAudioExporter {
		private SoX sox;

		public FLACExporter(SoX sox) {
			this.sox = sox;
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			sox.WriteFile(lwav, Path.Combine(output_dir, original_filename_no_ext + ".flac"));
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "FLAC (SoX)";
		}
	}
}
