using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.NintendoWare;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class RSTMExporter : VGAudioExporter {
		private readonly BxstmConfiguration _configuration;

		/// <summary>
		/// Creates a new RSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="configuration">The BxSTM configuration to use</param>
		public RSTMExporter(BxstmConfiguration configuration = null) {
			_configuration = configuration;
		}

		protected override byte[] GetData(AudioData audio) {
			return new BrstmWriter().GetFile(audio, _configuration);
		}

		protected override string GetExtension() {
			return ".brstm";
		}
	}
}
