using System;

namespace LoopingAudioConverter.PCM {
	/// <summary>
	/// Represents data stored in a lossy compressed format and not yet decoded to PCM.
	/// </summary>
	public interface IAudio : IDisposable { }
}
