using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using System;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter {
	/// <summary>
	/// A wrapper for PCM16Audio to conform to BrawlLib's IAudioStream interface.
	/// IAudioStream maintains a "current position" in the audio stream, while PCM16Audio does not, so this class handles SamplePosition, Wrap, and ReadSamples.
	/// </summary>
	public class PCM16LoopWrapper : IAudioStream {
		private readonly PCM16Audio lwav;

		public PCM16LoopWrapper(PCM16Audio lwav) {
			this.lwav = lwav.Channels > 2
				? lwav.MixToMono()
				: lwav;
		}

		public int BitsPerSample => 16;
		public int Channels => lwav.Channels;
		public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;
		public int Frequency => lwav.SampleRate;

		public bool IsLooping {
			get {
				return lwav.Looping;
			}
			set {
				lwav.Looping = value;
			}
		}

		public int LoopEndSample {
			get {
				return lwav.LoopEnd;
			}
			set {
				lwav.LoopEnd = value;
			}
		}

		public int LoopStartSample {
			get {
				return lwav.LoopStart;
			}
			set {
				lwav.LoopStart = value;
			}
		}

		public int ReadSamples(IntPtr destAddr, int numSamples) {
			if (SamplePosition + numSamples > Samples) {
				numSamples = Samples - SamplePosition;
			}

			Marshal.Copy(lwav.Samples, SamplePosition * Channels, destAddr, numSamples * Channels);
			SamplePosition += numSamples;
			return numSamples;
		}

		unsafe int IAudioStream.ReadSamples(VoidPtr destAddr, int numSamples) => ReadSamples(destAddr, numSamples);

		public int SamplePosition { get; set; }

		public int Samples => lwav.Samples.Length / lwav.Channels;

		public void Wrap() {
			SamplePosition = LoopStartSample;
		}

		public void Dispose() {
			return;
		}
	}
}
