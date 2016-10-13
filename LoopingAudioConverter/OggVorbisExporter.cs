using RSTMLib.WAV;
using System;
using System.IO;
using System.Linq;
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
            string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");

            // Don't re-encode if the original input file was also Ogg Vorbis
            if (new string[] { ".ogg", ".logg" }.Contains(Path.GetExtension(lwav.OriginalFilePath), StringComparer.InvariantCultureIgnoreCase)) {
                File.Copy(lwav.OriginalFilePath, output_filename, true);
            } else {
                sox.WriteFile(lwav, output_filename, encodingParameters);
            }
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
