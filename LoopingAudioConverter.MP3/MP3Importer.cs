using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MP3 {
	public class MP3Importer : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("mp3", StringComparison.InvariantCultureIgnoreCase);
		}

		public Task<PCM16Audio> ReadFileAsync(string filename) {
			byte[] mp3data = File.ReadAllBytes(filename);
			return Task.Run(() => {
				PCM16Audio a = MP3Audio.Read(mp3data);
				return a;
			});
		}
	}
}
