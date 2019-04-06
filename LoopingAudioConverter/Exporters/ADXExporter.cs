using VGAudio.Containers.Adx;
using VGAudio.Formats;

namespace LoopingAudioConverter {
	public class ADXExporter : VGAudioExporter {
		protected override byte[] GetData(AudioData audio) {
			return new AdxWriter().GetFile(audio);
		}

		protected override string GetExtension() {
			return ".adx";
		}
	}
}
