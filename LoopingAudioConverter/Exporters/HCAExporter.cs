using VGAudio.Containers.Hca;
using VGAudio.Containers.Hps;
using VGAudio.Formats;

namespace LoopingAudioConverter {
	public class HCAExporter : VGAudioExporter {
		protected override byte[] GetData(AudioData audio) {
			return new HcaWriter().GetFile(audio);
		}

		protected override string GetExtension() {
			return ".hca";
		}
	}
}
