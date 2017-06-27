using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.Bxstm;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class FSTMExporter : VGAudioExporter {
        private BxstmCodec encoding;

        /// <summary>
        /// Creates a new FSTMExporter instance that uses the given encoding when it has to re-encode a file.
        /// </summary>
        /// <param name="defaultEncoding">The encoding to use</param>
        public FSTMExporter(BxstmCodec defaultEncoding) {
            this.encoding = defaultEncoding;
        }

        protected override byte[] GetData(AudioData audio) {
            return new BfstmWriter().GetFile(audio, new BfstmConfiguration { Codec = this.encoding });
        }

        protected override string GetExtension() {
            return ".bfstm";
        }
    }
}
