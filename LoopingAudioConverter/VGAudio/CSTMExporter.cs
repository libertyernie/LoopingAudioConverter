using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.Bxstm;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class CSTMExporter : VGAudioExporter {
		private BxstmCodec encoding;

		/// <summary>
		/// Creates a new CSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use</param>
		public CSTMExporter(BxstmCodec defaultEncoding) {
			this.encoding = defaultEncoding;
		}

		protected override byte[] GetData(AudioData audio) {
			return new BcstmWriter().GetFile(audio, new BcstmConfiguration { Codec = this.encoding });
		}

		protected override string GetExtension() {
			return ".bcstm";
		}
	}
}
