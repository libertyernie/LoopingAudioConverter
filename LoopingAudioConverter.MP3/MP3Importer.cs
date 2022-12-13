using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MP3 {
	public class MP3Importer : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("mp3", StringComparison.InvariantCultureIgnoreCase);
		}

		public bool SharesCodecsWith(IAudioExporter exporter) => exporter is MP3Exporter;

		public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
			await Task.Yield();
			byte[] mp3data = File.ReadAllBytes(filename);
			var mp3 = new MP3Audio(mp3data);
			return mp3.Decode();
		}

		public IEnumerable<IAudio> TryReadFile(string filename) {
			byte[] mp3data = File.ReadAllBytes(filename);
			yield return new MP3Audio(mp3data);
		}
	}
}
