using BrawlLib.Wii.Audio;
using RSTMLib.WAV;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class CSTMExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			IProgressTracker pw = null;
			if (progressTracker != null) pw = new EncodingProgressWrapper(progressTracker);

			byte[] data = CSTMConverter.EncodeToByteArray(new PCM16AudioStream(lwav), pw);
			if (pw.Cancelled) throw new AudioExporterException("CSTM export cancelled");
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".bcstm"), data);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BCSTM (BrawlLib)";
		}
	}
}
