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
		private MultipleProgressTracker t;

		public RSTMExporter() {
			new Task(() => {
				t = new MultipleProgressTracker();
				t.Show();
			}).Start();
		}

		public void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext) {
			var pw = t.Add(original_filename_no_ext);
			FileMap map = RSTMConverter.Encode(new LWAVAudioStream(lwav), pw);
			if (pw.Cancelled) throw new AudioExporterException("RSTM export cancelled");
			File.Copy(map.FilePath, Path.Combine(output_dir, original_filename_no_ext + ".brstm"), true);
		}

		public Task WriteFileAsync(LWAV lwav, string output_dir, string original_filename_no_ext) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "RSTM (BrawlLib)";
		}
	}
}
