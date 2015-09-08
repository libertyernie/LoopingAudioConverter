using BrawlLib.IO;
using BrawlLib.Wii.Audio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class RSTMExporter : IAudioExporter {
		private ConsoleProgressTracker cpt = new ConsoleProgressTracker();

		public void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext) {
			IProgressTracker pw = cpt.Add(original_filename_no_ext);

			FileMap map = RSTMConverter.Encode(new LWAVAudioStream(lwav), pw);
			if (pw.Cancelled) throw new AudioExporterException("RSTM export cancelled");
			File.Copy(map.FilePath, Path.Combine(output_dir, original_filename_no_ext + ".brstm"), true);
		}

		public Task WriteFileAsync(LWAV lwav, string output_dir, string original_filename_no_ext) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "BRSTM (BrawlLib)";
		}
	}
}
