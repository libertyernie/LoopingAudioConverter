using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class FSTMExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			using (var pw = new ProgressWindow(null, "BrawlLib Audio Encoder", $"Encoding {original_filename_no_ext}", true))
			using (FileMap rstm = RSTMConverter.Encode(new PCM16AudioStream(lwav), pw, WaveEncoding.ADPCM)) {
				if (pw.Cancelled) throw new AudioExporterException("FSTM export cancelled");
				using (RSTMNode node = (RSTMNode)NodeFactory.FromSource(null, new DataSource(rstm))) {
					node.Export(Path.Combine(output_dir, original_filename_no_ext + ".bfstm"));
				}
			}
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
