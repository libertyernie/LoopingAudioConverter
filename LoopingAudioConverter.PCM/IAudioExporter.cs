using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoopingAudioConverter.PCM {
	public class AudioExporterException : Exception {
		public AudioExporterException(string message) : base(message) { }
	}

	public interface IAudioExporter {
		Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress = null);
	}
}
