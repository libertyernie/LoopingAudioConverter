using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Brawl {
	public class FSTMExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			byte[] rstmData = new RSTMExporter(WaveEncoding.ADPCM).Encode(lwav, original_filename_no_ext);
			byte[] fstmData = FSTMConverter.FromRSTM(rstmData);
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".bfstm"), fstmData);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BFSTM (BrawlLib)";
		}
	}
}
