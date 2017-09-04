using System;
using System.IO;
using System.Linq;
using VGAudio.Containers.Bxstm;
using VGAudio.Containers.Dsp;
using VGAudio.Containers.Hps;
using VGAudio.Containers.Idsp;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class VGAudioImporter : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			foreach (string s in new string[] {
				"brstm", "bcstm", "bfstm",
				"dsp", "idsp", "hps"
			}) {
				if (extension.Equals(s, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			return false;
		}

		private static AudioData Read(byte[] data, string filename) {
			string extension = Path.GetExtension(filename).ToLowerInvariant();
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			switch (extension) {
				case "brstm":
					return new BrstmReader().Read(data);
				case "bcstm":
					return new BcstmReader().Read(data);
				case "bfstm":
					return new BfstmReader().Read(data);
				case "dsp":
					return new DspReader().Read(data);
				case "idsp":
					return new IdspReader().Read(data);
				case "hps":
					return new HpsReader().Read(data);
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
				return PCM16Factory.FromAudioData(Read(data, filename));
			} catch (Exception e) {
				throw new AudioImporterException("Could not convert from B" + (char)data[0] + "STM: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "VGAudio";
		}
	}
}
