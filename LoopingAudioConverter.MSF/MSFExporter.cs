using LoopingAudioConverter.PCM;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSF {
	public class MSFExporter : IAudioExporter {
		private readonly bool big_endian;

		public MSFExporter(bool big_endian = true) {
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
