using VGAudio.Containers.Hca;
using VGAudio.Containers.Hps;
using VGAudio.Formats;

namespace LoopingAudioConverter {
	public class HCAExporter : VGAudioExporter {
		private readonly HcaConfiguration _configuration;

		public HCAExporter(HcaConfiguration configuration = null) {
			_configuration = configuration;
		}

		protected override byte[] GetData(AudioData audio) {
			return new HcaWriter().GetFile(audio, _configuration);
		}

		protected override string GetExtension() {
			return ".hca";
		}
	}
}
