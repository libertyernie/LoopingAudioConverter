using RSTMLib;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class RSTMExporter : IAudioExporter {
		public void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			IProgressTracker pw = null;
			if (progressTracker != null) pw = new EncodingProgressWrapper(progressTracker);

			byte[] data = RSTMConverter.EncodeToByteArray(new LWAVAudioStream(lwav), pw);
			if (pw.Cancelled) throw new AudioExporterException("RSTM export cancelled");
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".brstm"), data);
		}

		public Task WriteFileAsync(LWAV lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BRSTM (BrawlLib)";
		}
	}
}
