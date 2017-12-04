using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.NintendoWare;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class CSTMExporter : VGAudioExporter {
		private NwCodec encoding;

		/// <summary>
		/// Creates a new CSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use</param>
		public CSTMExporter(NwCodec defaultEncoding) {
			this.encoding = defaultEncoding;
		}

		protected override byte[] GetData(AudioData audio) {
			return new BCFstmWriter(NwTarget.Ctr).GetFile(audio, new BxstmConfiguration { Codec = this.encoding });
		}

		protected override string GetExtension() {
			return ".bcstm";
		}
	}
}
