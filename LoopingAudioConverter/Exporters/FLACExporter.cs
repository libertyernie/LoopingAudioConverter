using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class FLACExporter : IAudioExporter {
		private readonly IEffectEngine effectEngine;

		public FLACExporter(IEffectEngine effectEngine) {
			this.effectEngine = effectEngine;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			await effectEngine.WriteFileAsync(lwav, Path.Combine(output_dir, original_filename_no_ext + ".flac"));
		}
	}
}
