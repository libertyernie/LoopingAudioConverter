using System;
using System.Threading.Tasks;

namespace LoopingAudioConverter.PCM {
	public class AudioImporterException : Exception {
		public AudioImporterException(string message) : base(message) { }
		public AudioImporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public interface IAudioHints {
		int RenderingSampleRate { get; }
		int? SampleCount { get; }
	}

	public interface IAudioImporter {
		bool SupportsExtension(string extension);
		bool SharesCodecsWith(IAudioExporter exporter);
		Task<PCM16Audio> ReadFileAsync(string filename, IAudioHints hints = null, IProgress<double> progress = null);
	}
}
