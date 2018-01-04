using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class RSTMExporter : IAudioExporter {
		public byte[] Encode(PCM16Audio lwav, string original_filename_no_ext) {
			if (lwav.OriginalPath != null) {
				switch (Path.GetExtension(lwav.OriginalPath).ToLowerInvariant()) {
					case ".brstm":
						return File.ReadAllBytes(lwav.OriginalPath);
					case ".bcstm":
						return CSTMConverter.ToRSTM(File.ReadAllBytes(lwav.OriginalPath));
					case ".bfstm":
						return FSTMConverter.ToRSTM(File.ReadAllBytes(lwav.OriginalPath));
				}
			}
			
			using (var pw = new ProgressWindow(null, "BrawlLib Audio Encoder", $"Encoding {original_filename_no_ext}", true))
			using (FileMap rstm = RSTMConverter.Encode(new PCM16AudioStream(lwav), pw, WaveEncoding.ADPCM)) {
				if (pw.Cancelled) throw new AudioExporterException("RSTM export cancelled");

				byte[] data = new byte[rstm.Length];
				Marshal.Copy(rstm.Address, data, 0, rstm.Length);
				return data;
			}
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".brstm"), Encode(lwav, original_filename_no_ext));
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BRSTM (BrawlLib)";
		}
	}
}
