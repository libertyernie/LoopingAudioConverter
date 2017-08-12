using System;
using System.Audio;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.Brawl {
	/// <summary>
	/// A wrapper for PCM16Audio to conform to BrawlLib's IAudioStream interface.
	/// IAudioStream maintains a "current position" in the audio stream, while PCM16Audio does not, so this class handles SamplePosition, Wrap, and ReadSamples.
	/// </summary>
	public class PCM16AudioStream : IAudioStream {
		private PCM16Audio lwav;

		public PCM16AudioStream(PCM16Audio lwav) {
			this.lwav = lwav;
		}

		public int BitsPerSample {
			get {
				return 16;
			}
		}

		public int Channels {
			get {
				return lwav.Channels;
			}
		}

		public WaveFormatTag Format {
			get {
				return WaveFormatTag.WAVE_FORMAT_PCM;
			}
		}

		public int Frequency {
			get {
				return lwav.SampleRate;
			}
		}

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

		public int ReadSamples(VoidPtr destAddr, int numSamples) {
			if (SamplePosition + numSamples > Samples) {
				numSamples = Samples - SamplePosition;
			}

			Marshal.Copy(lwav.Samples, SamplePosition * Channels, destAddr, numSamples * Channels);
			SamplePosition += numSamples;
			return numSamples;
		}

		public int SamplePosition { get; set; }

		public int Samples {
			get {
				return lwav.Samples.Length / lwav.Channels;
			}
		}

		public void Wrap() {
			SamplePosition = LoopStartSample;
		}

		public void Dispose() {
			return;
		}
	}
}
