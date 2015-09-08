using System;
using System.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LoopingAudioConverter.Brawl {
	public class LWAVAudioStream : IAudioStream {
		private LWAV lwav;

		public LWAVAudioStream(LWAV lwav) {
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
				throw new NotImplementedException();
			}
		}

		public int LoopEndSample {
			get {
				return lwav.LoopEnd;
			}
			set {
				throw new NotImplementedException();
			}
		}

		public int LoopStartSample {
			get {
				return lwav.LoopStart;
			}
			set {
				throw new NotImplementedException();
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
