using System;

namespace LoopingAudioConverter.PCM {
	public interface IAudio : IDisposable {
		/// <summary>
		/// Whether the file is known to loop.
		/// </summary>
		bool Looping { get; set; }

		/// <summary>
		/// The start of the loop, in samples.
		/// </summary>
		int LoopStart { get; set; }

		/// <summary>
		/// The end of the loop, in samples.
		/// </summary>
		int LoopEnd { get; set; }
	}
}
