using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class BrawlLibRSTMExporter : IAudioExporter {
		private readonly WaveEncoding _waveEncoding;

		public BrawlLibRSTMExporter(WaveEncoding waveEncoding) {
			_waveEncoding = waveEncoding;
		}

		public unsafe void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			var wrapper = new PCM16LoopWrapper(lwav);
			using (var fileMap = RSTMConverter.Encode(wrapper, null, _waveEncoding)) {
				using (var inputStream = new UnmanagedMemoryStream((byte*)fileMap.Address.address, fileMap.Length))
				using (var outputStream = new FileStream(Path.Combine(output_dir, original_filename_no_ext + ".brstm"), FileMode.Create, FileAccess.Write)) {
					inputStream.CopyTo(outputStream);
				}
			}
		}

		Task IAudioExporter.WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			return Task.Run(() => WriteFile(lwav, output_dir, original_filename_no_ext));
		}
	}
}
