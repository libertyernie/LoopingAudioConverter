using LoopingAudioConverter.Immutable;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSU1 {
	public class MSU1Converter : IAudioImporter, IAudioExporter {
		public Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
			using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			using (var br = new BinaryReader(fs)) {
				foreach (char c in "MSU1") {
					byte x = br.ReadByte();
					if (x != c) {
						throw new AudioImporterException("This is not a valid MSU-1 .pcm file");
					}
				}

				uint loopStart = br.ReadUInt32();

				short[] sampleData = new short[(fs.Length - 8) / sizeof(short)];
				for (int i = 0; i < sampleData.Length; i++) {
					sampleData[i] = br.ReadInt16();
				}

				return Task.FromResult(new PCM16Audio(
					new PCMData(2, 44100, sampleData),
					loopStart == 0
						? LoopType.NonLooping
						: LoopType.NewLooping(checked((int)loopStart), sampleData.Length / 2)));
			}
		}

		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("pcm", StringComparison.InvariantCultureIgnoreCase);
		}

		bool IAudioImporter.SharesCodecsWith(IAudioExporter exporter) => false;

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			if (lwav.Channels != 2 || lwav.SampleRate != 44100) {
				throw new AudioExporterException("MSU-1 output must be 2-channel audio at a sample rate of 44100Hz.");
			}

			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".pcm");
			using (var fs = new FileStream(output_filename, FileMode.Create, FileAccess.Write))
			using (var bw = new BinaryWriter(fs)) {
				foreach (char c in "MSU1") {
					bw.Write((byte)c);
				}

				if (lwav.Looping) {
					bw.Write(lwav.LoopStart);
				} else {
					bw.Write(0);
				}
				
				foreach (short sample in lwav.Samples) {
					bw.Write(sample);
				}
			}
			return Task.FromResult(0);
		}
	}
}
