using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.NintendoWare;
using VGAudio.Formats;
using VGAudio.Utilities;

namespace LoopingAudioConverter {
	public class FSTMExporter : VGAudioExporter {
		private readonly BxstmConfiguration _configuration;

		/// <summary>
		/// Creates a new FSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use</param>
		public FSTMExporter(BxstmConfiguration configuration = null) {
			_configuration = configuration;
		}

		protected override byte[] GetData(AudioData audio) {
			return new BCFstmWriter(NwTarget.Cafe).GetFile(audio, _configuration);
		}

		protected override string GetExtension() {
			return ".bfstm";
		}
	}
}
