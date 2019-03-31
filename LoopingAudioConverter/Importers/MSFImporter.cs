using MSFContainerLib;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MSFImporter : IAudioImporter {
		public string GetImporterName() => "MSF";

		public Task<PCM16Audio> ReadFileAsync(string filename) {
			byte[] data = File.ReadAllBytes(filename);
			try {
				IPcmAudioSource<short> msf = MSF.Parse(data);
				return Task.FromResult(new PCM16Audio(msf.Channels, msf.SampleRate, msf.SampleData.ToArray(), msf.LoopStartSample, msf.LoopStartSample + msf.LoopSampleCount, !msf.IsLooping));
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
