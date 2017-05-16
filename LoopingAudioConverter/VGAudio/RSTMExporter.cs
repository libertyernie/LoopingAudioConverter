using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers;
using VGAudio.Containers.Bxstm;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
    public class RSTMExporter : VGAudioExporter {
        private BxstmCodec encoding;

        /// <summary>
        /// Creates a new RSTMExporter instance that uses the given encoding when it has to re-encode a file.
        /// </summary>
        /// <param name="defaultEncoding">The encoding to use</param>
        public RSTMExporter(BxstmCodec defaultEncoding) {
            this.encoding = defaultEncoding;
        }

        protected override byte[] GetData(AudioData audio) {
            return new BrstmWriter().GetFile(audio, new BrstmConfiguration { Codec = this.encoding });
        }

        protected override string GetExtension() {
            return ".brstm";
        }
	}
}
