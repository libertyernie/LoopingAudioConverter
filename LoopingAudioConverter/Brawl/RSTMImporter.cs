using System;
using System.IO;
using System.Linq;
using VGAudio.Containers;

namespace LoopingAudioConverter.Brawl {
	public class RSTMImporter : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("brstm", StringComparison.InvariantCultureIgnoreCase)
				|| extension.Equals("bcstm", StringComparison.InvariantCultureIgnoreCase)
				|| extension.Equals("bfstm", StringComparison.InvariantCultureIgnoreCase);
		}

        private static AudioWithConfig ReadWithConfig(byte[] data) {
            switch ((char)data[0]) {
                case 'R':
                    return new BrstmReader().ReadWithConfig(data);
                case 'C':
                    return new BcstmReader().ReadWithConfig(data);
                case 'F':
                    return new BfstmReader().ReadWithConfig(data);
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
                AudioWithConfig audio = ReadWithConfig(data);
                byte[] wav = new WaveWriter().GetFile(audio.Audio, audio.Configuration);

                return PCM16Factory.FromByteArray(wav);
			} catch (Exception e) {
				throw new AudioImporterException("Could not convert from B" + (char)data[0] + "STM: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "BrawlLib/RSTMLib";
		}
	}
}
