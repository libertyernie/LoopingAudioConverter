using LoopingAudioConverter.MSF;
using LoopingAudioConverter.PCM;
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

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			var msf = big_endian
				? (MSF_PCM16)MSF_PCM16BE.FromPCM(lwav)
				: MSF_PCM16LE.FromPCM(lwav);
			string outPath = Path.Combine(output_dir, original_filename_no_ext + ".msf");
			File.WriteAllBytes(outPath, msf.Export());
			return Task.CompletedTask;
		}
	}
}
