using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class AudioImporterException : Exception {
		public AudioImporterException(string message) : base(message) {}
		public AudioImporterException(string message, Exception innerException) : base(message, innerException) { }
	}

	public interface IAudioImporter {
		bool SupportsExtension(string extension);
		LWAV ReadFile(string filename);
		string GetImporterName();
	}

	public interface IAudioExporter {
		void WriteFile(LWAV lwav, string output_dir, string original_filename_no_ext);
		string GetExporterName();
	}
}
