using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.NintendoWare;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class CSTMExporter : VGAudioExporter {
		private readonly BxstmConfiguration _configuration;

		/// <summary>
		/// Creates a new CSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use</param>
		public CSTMExporter(BxstmConfiguration configuration = null) {
			_configuration = configuration;
		}

		protected override byte[] GetData(AudioData audio) {
			return new BCFstmWriter(NwTarget.Ctr).GetFile(audio, _configuration);
		}

		protected override string GetExtension() {
			return ".bcstm";
		}
	}
}
