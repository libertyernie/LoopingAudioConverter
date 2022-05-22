using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using LoopingAudioConverter.PCM;
using System;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.BrawlLib {
	/// <summary>
	/// A wrapper for PCM16Audio to conform to BrawlLib's IAudioStream interface.
	/// IAudioStream maintains a "current position" in the audio stream, while PCM16Audio does not, so this class handles SamplePosition, Wrap, and ReadSamples.
	/// </summary>
	public class PCM16LoopWrapper : IAudioStream {
		private readonly PCM16Audio original, mixed;

		public PCM16LoopWrapper(PCM16Audio lwav) {
			original = lwav;
			mixed = lwav.Channels > 2
				? lwav.MixToMono()
				: lwav;
		}

		public int BitsPerSample => 16;
		public int Channels => mixed.Channels;
		public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;
		public int Frequency => mixed.SampleRate;

		public bool IsLooping {
			get {
				return original.Looping;
			}
			set {
				original.Looping = value;
			}
		}

		public int LoopEndSample {
			get {
				return original.LoopEnd;
			}
			set {
				original.LoopEnd = value;
			}
		}

		public int LoopStartSample {
			get {
				return original.LoopStart;
			}
			set {
				original.LoopStart = value;
			}
		}

		public int ReadSamples(IntPtr destAddr, int numSamples) {
			if (SamplePosition + numSamples > Samples) {
				numSamples = Samples - SamplePosition;
			}

			Marshal.Copy(mixed.Samples, SamplePosition * Channels, destAddr, numSamples * Channels);
			SamplePosition += numSamples;
			return numSamples;
		}

		unsafe int IAudioStream.ReadSamples(VoidPtr destAddr, int numSamples) => ReadSamples(destAddr, numSamples);

		public int SamplePosition { get; set; }

		public int Samples => mixed.Samples.Length / mixed.Channels;

		public void Wrap() {
			SamplePosition = LoopStartSample;
		}

		public void Dispose() {
			return;
		}
	}
}
