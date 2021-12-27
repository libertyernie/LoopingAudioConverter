using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSF {
	public class MSFImporter : IAudioImporter {
		public Task<PCM16Audio> ReadFileAsync(string filename) {
			byte[] data = File.ReadAllBytes(filename);
			try {
				var msf = MSF.Parse(data);
				var lwav = msf.Decode();
				return Task.FromResult(lwav);
			} catch (NotSupportedException) {
				throw new AudioImporterException("Cannot read MSF file (unsupported codec?)");
			}
		}

		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("msf", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
