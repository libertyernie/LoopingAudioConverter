using BrawlLib.Wii.Audio;
using System;
using System.IO;
using System.Linq;

namespace LoopingAudioConverter.Brawl {
	public class RSTMImporter : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("brstm", StringComparison.InvariantCultureIgnoreCase)
				|| extension.Equals("bcstm", StringComparison.InvariantCultureIgnoreCase)
				|| extension.Equals("bfstm", StringComparison.InvariantCultureIgnoreCase);
		}

		public PCM16Audio ReadFile(string filename) {
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			byte[] data = File.ReadAllBytes(filename);
			if (data.Length == 0) {
				throw new AudioImporterException("Empty input file");
			}
			try {
				if (data[0] == 'C') {
					data = CSTMConverter.ToRSTM(data);
				} else if (data[0] == 'F') {
					data = FSTMConverter.ToRSTM(data);
				}

				return PCM16Factory.FromAudioStream(RSTMConverter.CreateStreams(data)[0]);
			} catch (Exception e) {
				throw new AudioImporterException("Could not convert from B" + (char)data[0] + "STM: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "BrawlLib/RSTMLib";
		}
	}
}
