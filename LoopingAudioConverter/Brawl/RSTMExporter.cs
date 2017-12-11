using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class RSTMExporter : IAudioExporter {
		private WaveEncoding encoding;

		/// <summary>
		/// Creates a new RSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use (ADPCM or PCM16)</param>
		public RSTMExporter(WaveEncoding defaultEncoding) {
			this.encoding = defaultEncoding;
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			using (var pw = new ProgressWindow(null, "BrawlLib Audio Encoder", $"Encoding {original_filename_no_ext}", true))
			using (FileMap data = RSTMConverter.Encode(new PCM16AudioStream(lwav), pw, encoding)) {
				if (pw.Cancelled) throw new AudioExporterException("RSTM export cancelled");
				File.Copy(data.FilePath, Path.Combine(output_dir, original_filename_no_ext + ".brstm"), true);
			}
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BRSTM (BrawlLib): " + encoding;
		}
	}
}
