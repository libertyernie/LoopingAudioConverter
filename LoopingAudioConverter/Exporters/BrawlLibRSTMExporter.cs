using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class BrawlLibRSTMExporter : IAudioExporter {
		public enum Container { RSTM, CSTM, FSTM }

		public class SilentProgressTracker : IProgressTracker {
			public float MinValue { get; set; }
			public float MaxValue { get; set; }
			public float CurrentValue { get; set; }
			public bool Cancelled { get; set; }

			public void Begin(float min, float max, float current) { }

			public void Cancel() => throw new NotImplementedException();

			public void Finish() { }

			public void Update(float value) { }
		}

		private readonly WaveEncoding _waveEncoding;
		private readonly Container _container;

		public BrawlLibRSTMExporter(WaveEncoding waveEncoding, Container container) {
			_waveEncoding = waveEncoding;
			_container = container;
		}

		private unsafe byte[] Encode(PCM16Audio lwav) {
			using (var ms = new MemoryStream()) {
				var wrapper = new PCM16LoopWrapper(lwav);

				using (var fileMap = RSTMConverter.Encode(wrapper, new SilentProgressTracker(), _waveEncoding))
				using (var inputStream = new UnmanagedMemoryStream((byte*)fileMap.Address.address, fileMap.Length)) {
					inputStream.CopyTo(ms);
				}

				return ms.ToArray();
			}
		}

		private byte[] Read(PCM16Audio lwav) {
			string orig_path = lwav.OriginalPath?.ToLowerInvariant() ?? "";
			if (orig_path.EndsWith(".brstm")) {
				return File.ReadAllBytes(orig_path);
			} else if (orig_path.EndsWith(".bcstm")) {
				return CSTMConverter.ToRSTM(File.ReadAllBytes(orig_path));
			} else if (orig_path.EndsWith(".bfstm")) {
				return FSTMConverter.ToRSTM(File.ReadAllBytes(orig_path));
			} else {
				return Encode(lwav);
			}
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			byte[] data = Read(lwav);
			string ext = ".brstm";
			if (_container == Container.CSTM) {
				data = CSTMConverter.FromRSTM(data);
				ext = ".bcstm";
			}
			if (_container == Container.FSTM) {
				data = FSTMConverter.FromRSTM(data);
				ext = ".bfstm";
			}
			File.WriteAllBytes(
				Path.Combine(output_dir, original_filename_no_ext + ext),
				data);
		}

		async Task IAudioExporter.WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			await Task.Yield();
			await Task.Run(() => WriteFile(lwav, output_dir, original_filename_no_ext));
		}
	}
}
