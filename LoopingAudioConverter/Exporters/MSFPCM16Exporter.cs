using MSFContainerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MSFPCM16Exporter : IAudioExporter {
		private readonly bool big_endian;

		public MSFPCM16Exporter(bool big_endian = true) {
			this.big_endian = big_endian;
		}

		public string GetExporterName() {
			return $"MSF (PCM16 {(big_endian ? "little endian" : "big endian")}";
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			var msf = MSF.FromAudioSource(lwav, big_endian: big_endian);
			string outPath = Path.Combine(output_dir, original_filename_no_ext + ".msf");
			File.WriteAllBytes(outPath, msf.Export());
			return Task.CompletedTask;
		}
	}
}
