using LoopingAudioConverter.PCM;
using System;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
    public sealed class VGAudioAudio : IAudio {
		public readonly AudioData AudioData;

		public VGAudioAudio(AudioData encoded) {
			AudioData = encoded;
		}

		public override string ToString() {
			return base.ToString() + " (VGAudio)";
        }

		void IDisposable.Dispose() { }
	}
}
