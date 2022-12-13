using System;
using System.Threading.Tasks;

namespace LoopingAudioConverter.PCM {
	/// <summary>
	/// Represents data stored in a lossy compressed format and not yet decoded to PCM.
	/// </summary>
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

		/// <summary>
		/// Decodes the file to 16-bit PCM.
		/// </summary>
		/// <returns>A PCM16Audio object with the decoded audio and the same loop pointss</returns>
		Task<PCM16Audio> DecodeAsync();
	}
}
