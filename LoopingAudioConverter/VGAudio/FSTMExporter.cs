using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.NintendoWare;
using VGAudio.Formats;
using VGAudio.Utilities;

namespace LoopingAudioConverter.VGAudio {
	public class FSTMExporter : VGAudioExporter {
		private readonly NwCodec encoding;
		private readonly Endianness? endianness;

		/// <summary>
		/// Creates a new FSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use</param>
		/// <param name="endianness">The endianness to use</param>
		public FSTMExporter(NwCodec defaultEncoding, Endianness? endianness = null) {
			this.encoding = defaultEncoding;
			this.endianness = endianness;
		}

		protected override byte[] GetData(AudioData audio) {
			return new BCFstmWriter(NwTarget.Cafe).GetFile(audio, new BxstmConfiguration { Codec = this.encoding, Endianness = this.endianness });
		}

		protected override string GetExtension() {
			return ".bfstm";
		}
	}
}
