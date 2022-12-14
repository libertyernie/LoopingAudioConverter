using System;
using System.Threading.Tasks;

namespace LoopingAudioConverter.PCM {
	public class AudioExporterException : Exception {
		public AudioExporterException(string message) : base(message) { }
	}

	public interface IPCMAudioExporter {
		Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress = null);
	}

	public interface IAudioExporter : IPCMAudioExporter {
		void TryWriteFile(IAudio audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext);
	}
}
