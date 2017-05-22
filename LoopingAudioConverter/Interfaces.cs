using System;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class AudioImporterException : Exception {
		public AudioImporterException(string message) : base(message) {}
		public AudioImporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public class AudioExporterException : Exception {
		public AudioExporterException(string message) : base(message) { }
		public AudioExporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public interface IAudioImporter {
		bool SupportsExtension(string extension);
		PCM16Audio ReadFile(string filename);
		string GetImporterName();
    }

    public interface IAudioExporter {
		void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext);
		Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext);
		string GetExporterName();
	}
}
