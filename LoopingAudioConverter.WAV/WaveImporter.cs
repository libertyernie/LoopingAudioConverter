using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.WAV {
	/// <summary>
	/// A wrapper around PCM16Factory that implements the IAudioImporter interface.
	/// For .wav files, it makes sense to read them directly when possible. If the file cannot be read, vgmstream or ffmpeg should be used instead.
	/// </summary>
	public class WaveImporter : IAudioImporter {
		private static string[] EXTENSIONS = new string[] { "wav", "lwav" };

		/// <summary>
		/// Returns whether this importer supports a given file extension: true for .wav and .lwav, false otherwise.
		/// </summary>
		/// <param name="extension">File extension, with or without leading period</param>
		/// <returns>true if the file might be readable using this importer; false if it's not and this importer should be skipped</returns>
		public bool SupportsExtension(string extension) {
			while (extension.StartsWith(".")) extension = extension.Substring(1);
			return EXTENSIONS.Any(s => s.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
		}

		public Task<PCM16Audio> ReadFileAsync(string filename, IProgress<double> progress) {
			try {
				return Task.FromResult(WaveConverter.FromByteArray(File.ReadAllBytes(filename)));
			} catch (WaveConverterException e) {
				throw new AudioImporterException(e.Message, e);
			}
		}
	}
}
