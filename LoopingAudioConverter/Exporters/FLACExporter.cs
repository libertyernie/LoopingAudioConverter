using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class FLACExporter : IAudioExporter {
		private SoX sox;

		public FLACExporter(SoX sox) {
			this.sox = sox;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			await sox.WriteFileAsync(lwav, Path.Combine(output_dir, original_filename_no_ext + ".flac"));
		}
	}
}
