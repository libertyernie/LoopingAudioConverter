using System;
using System.Threading.Tasks;

namespace LoopingAudioConverter.PCM {
	public class AudioImporterException : Exception {
		public AudioImporterException(string message) : base(message) { }
		public AudioImporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public interface IAudioImporter {
		bool SupportsExtension(string extension);
		Task<PCM16Audio> ReadFileAsync(string filename, IProgress<double> progress = null);
	}

	public interface IRenderingAudioImporter : IAudioImporter {
		int? SampleRate { set; }
	}

	public interface IOpinionatedAudioImporter : IAudioImporter {
		bool SharesCodecsWith(IAudioExporter exporter);
	}
}
