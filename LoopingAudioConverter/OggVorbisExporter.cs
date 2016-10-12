using RSTMLib.WAV;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class OggVorbisExporter : IAudioExporter {
		private SoX sox;
        private string encodingParameters;

		public OggVorbisExporter(SoX sox, string encodingParameters = null) {
			this.sox = sox;
            this.encodingParameters = encodingParameters;
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			sox.WriteFile(lwav, Path.Combine(output_dir, original_filename_no_ext + ".ogg"), encodingParameters);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "Ogg Vorbis (SoX)";
		}
	}
}
