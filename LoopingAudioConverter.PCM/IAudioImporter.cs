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

	public interface IGenericAudioImporter {
		bool SupportsExtension(string extension);
	}

	public interface IPCMAudioImporter : IGenericAudioImporter {
		Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints = null, IProgress<double> progress = null);
	}

	public interface ICompressedAudioImporter : IGenericAudioImporter {
		IEnumerable<object> TryReadFile(string filename);
	}

	public interface IAudioImporter : IPCMAudioImporter, ICompressedAudioImporter { }
}
