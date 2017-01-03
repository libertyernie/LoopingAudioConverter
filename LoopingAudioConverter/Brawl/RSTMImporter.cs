using DspAdpcm;
using DspAdpcm.Adpcm.Formats;
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

            LoopingTrackStream stream = null;
			try {
				if (data[0] == 'C') {
                    stream = new Bcstm(data).AudioStream;
				} else if (data[0] == 'F') {
					stream = new Brstm(data).AudioStream;
				} else {
                    stream = new Brstm(data).AudioStream;
                }
			} catch (Exception e) {
				throw new AudioImporterException("Could not convert from B" + (char)data[0] + "STM: " + e.Message);
			}

            return PCM16Factory.FromAudioStream(stream);
		}

		public string GetImporterName() {
			return "LibDspAdpcm";
		}
	}
}
