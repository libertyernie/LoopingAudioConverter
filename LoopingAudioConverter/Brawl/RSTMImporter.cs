using System;
using System.IO;
using System.Linq;
using VGAudio.Containers;
using VGAudio.Formats;

namespace LoopingAudioConverter.Brawl {
	public class RSTMImporter : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("brstm", StringComparison.InvariantCultureIgnoreCase)
				|| extension.Equals("bcstm", StringComparison.InvariantCultureIgnoreCase)
				|| extension.Equals("bfstm", StringComparison.InvariantCultureIgnoreCase);
		}

        private static AudioData Read(byte[] data) {
            switch ((char)data[0]) {
                case 'R':
                    return new BrstmReader().Read(data);
                case 'C':
                    return new BcstmReader().Read(data);
                case 'F':
                    return new BfstmReader().Read(data);
                default:
                    throw new NotImplementedException();
            }
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
                return PCM16Factory.FromAudioData(Read(data));
			} catch (Exception e) {
				throw new AudioImporterException("Could not convert from B" + (char)data[0] + "STM: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "BrawlLib/RSTMLib";
		}
	}
}
