using VGAudio.Containers.Adx;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class ADXExporter : VGAudioExporter {
		private readonly AdxConfiguration _configuration;

		public ADXExporter(AdxConfiguration configuration = null) {
			_configuration = configuration;
		}

		protected override byte[] GetData(AudioData audio) {
			return new AdxWriter().GetFile(audio, _configuration);
		}

		protected override string GetExtension() {
			return ".adx";
		}
	}
}
