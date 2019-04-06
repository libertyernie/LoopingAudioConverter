using VGAudio.Containers.Idsp;
using VGAudio.Formats;

namespace LoopingAudioConverter {
	public class IDSPExporter : VGAudioExporter {
		protected override byte[] GetData(AudioData audio) {
			return new IdspWriter().GetFile(audio);
		}

		protected override string GetExtension() {
			return ".idsp";
		}
	}
}
