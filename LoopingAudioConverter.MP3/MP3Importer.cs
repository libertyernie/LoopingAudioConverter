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

		public bool SharesCodecsWith(IAudioExporter exporter) => exporter is MP3Exporter;

		public Task<PCM16Audio> ReadFileAsync(string filename, IAudioHints hints, IProgress<double> progress) {
			byte[] mp3data = File.ReadAllBytes(filename);
			return Task.Run(() => {
				PCM16Audio a = MP3Audio.Read(mp3data);
				return a;
			});
		}
	}
}
