using MSFContainerLib;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MSFImporter : IAudioImporter {
		public Task<PCM16Audio> ReadFileAsync(string filename) {
			byte[] data = File.ReadAllBytes(filename);
			try {
				IPcmAudioSource<short> msf = MSF.Parse(data);
				var lwav = new PCM16Audio(
					msf.Channels,
					msf.SampleRate,
					msf.SampleData.ToArray(),
					msf.LoopStartSample,
					msf.LoopStartSample + msf.LoopSampleCount,
					!msf.IsLooping);
				if (msf is MSF_MP3 mp3) {
					lwav.OriginalMP3 = mp3.Body;
				}
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
