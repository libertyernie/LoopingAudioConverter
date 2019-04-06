using VGAudio.Containers.Hps;
using VGAudio.Formats;

namespace LoopingAudioConverter {
	public class HPSExporter : VGAudioExporter {
		protected override byte[] GetData(AudioData audio) {
			return new HpsWriter().GetFile(audio);
		}

		protected override string GetExtension() {
			return ".hps";
		}
	}
}
