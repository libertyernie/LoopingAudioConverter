using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class FLACExporter : IAudioExporter {
		private SoX sox;

		public FLACExporter(SoX sox) {
			this.sox = sox;
		}

		public void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext) {
			sox.WriteFile(lwav, Path.Combine(output_dir, original_filename_no_ext + ".flac"));
		}

		public Task WriteFileAsync(LWAV lwav, string output_dir, string original_filename_no_ext) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "FLAC (SoX)";
		}
	}
}
