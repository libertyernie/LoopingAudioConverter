using LoopingAudioConverter.PCM;
using System;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class AudioImporterException : Exception {
		public AudioImporterException(string message) : base(message) { }
		public AudioImporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public class AudioExporterException : Exception {
		public AudioExporterException(string message) : base(message) { }
	}

	public interface IAudioImporter {
		bool SupportsExtension(string extension);
		Task<PCM16Audio> ReadFileAsync(string filename);
	}

	public interface IRenderingAudioImporter : IAudioImporter {
		int? SampleRate { set; }
	}

	public interface IAudioExporter {
		Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext);
	}
}
