using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoopingAudioConverter.PCM {
	public class AudioImporterException : Exception {
		public AudioImporterException(string message) : base(message) { }
		public AudioImporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public interface IRenderingHints {
		int RenderingSampleRate { get; }
		TimeSpan? RequiredDecodingDuration { get; }
	}

	public interface IPCMAudioImporter {
		bool SupportsExtension(string extension);
		Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints = null, IProgress<double> progress = null);
	}

	public interface IAudioImporter : IPCMAudioImporter {
		IEnumerable<object> TryReadFile(string filename);
	}
}
