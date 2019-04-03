using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	/// <summary>
	/// A wrapper around PCM16Factory that implements the IAudioImporter interface.
	/// For .wav files, it makes sense to read them directly when possible instead of invoking SoX to convert them. If the file cannot be read, vgmstream (or SoX, if vgmstream fails) should be used to convert the file to a format PCM16Factory will recognize.
	/// </summary>
	public class WAVImporter : IAudioImporter {
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

		public Task<PCM16Audio> ReadFileAsync(string filename) {
			try {
				return Task.FromResult(PCM16Factory.FromByteArray(File.ReadAllBytes(filename)));
			} catch (PCM16FactoryException e) {
				throw new AudioImporterException(e.Message, e);
			}
		}
	}
}
