using BrawlLib.Wii.Audio;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.BrawlLib {
	public class BrawlLibRWAVExporter : IAudioExporter {
		public unsafe void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string outputPath = Path.Combine(output_dir, original_filename_no_ext + ".brwav");
			var wrapper = new PCM16LoopWrapper(lwav);

			using (var fileMap = RWAVConverter.Encode(wrapper, null))
			using (var inputStream = new UnmanagedMemoryStream((byte*)fileMap.Address.address, fileMap.Length))
			using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write)) {
				inputStream.CopyTo(outputStream);
			}
		}

		Task IAudioExporter.WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			return Task.Run(() => WriteFile(lwav, output_dir, original_filename_no_ext));
		}
	}
}
