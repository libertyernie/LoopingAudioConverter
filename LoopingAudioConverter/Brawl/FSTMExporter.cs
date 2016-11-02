using BrawlLib.Wii.Audio;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class FSTMExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			IProgressTracker pw = null;
			if (progressTracker != null) pw = new EncodingProgressWrapper(progressTracker);
            
            byte[] data;
            switch (Path.GetExtension(lwav.OriginalFilePath ?? "").ToLowerInvariant()) {
                case ".brstm":
                    data = FSTMConverter.FromRSTM(File.ReadAllBytes(lwav.OriginalFilePath));
                    break;
                case ".bcstm":
                    data = FSTMConverter.FromRSTM(CSTMConverter.ToRSTM(File.ReadAllBytes(lwav.OriginalFilePath)));
                    break;
                case ".bfstm":
                    data = File.ReadAllBytes(lwav.OriginalFilePath);
                    break;
                default:
                    data = FSTMConverter.EncodeToByteArray(new PCM16AudioStream(lwav), pw);
			        if (pw.Cancelled) throw new AudioExporterException("FSTM export cancelled");
                    break;
            }
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".bfstm"), data);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BFSTM (BrawlLib)";
		}
	}
}
