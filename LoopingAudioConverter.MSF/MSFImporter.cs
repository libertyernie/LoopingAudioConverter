using LoopingAudioConverter.MP3;
using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSF {
	public class MSFImporter : IAudioImporter {
		public Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
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

		public IEnumerable<IAudio> TryReadFile(string filename) {
			byte[] data = File.ReadAllBytes(filename);
			var msf = MSF.Parse(data);
			yield return msf.Read();
		}
	}
}
