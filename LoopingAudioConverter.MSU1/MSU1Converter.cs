using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSU1 {
	public class MSU1Converter : IAudioImporter, IAudioExporter {
		public Task<PCM16Audio> ReadFileAsync(string filename) {
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

				var pcm16 = new PCM16Audio(2, 44100, sampleData, checked((int)loopStart));
				if (loopStart == 0) {
					// This might be a non-looping song, or a song that loops without any lead-in.
					pcm16.Looping = false;
				}
				return Task.FromResult(pcm16);
			}
		}

		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("pcm", StringComparison.InvariantCultureIgnoreCase);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
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
