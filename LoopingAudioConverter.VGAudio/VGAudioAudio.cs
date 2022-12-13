using LoopingAudioConverter.PCM;
using System;
using System.Linq;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
    public sealed class VGAudioAudio : IAudio {
        private readonly AudioData _audioData;

		public bool Looping { get; set; }
		public int LoopStart { get; set; }
		public int LoopEnd { get; set; }

		public VGAudioAudio(AudioData encoded) {
			_audioData = encoded;

			Looping = encoded.GetAllFormats().Select(x => x.Looping).Distinct().Single();
			LoopStart = encoded.GetAllFormats().Select(x => x.LoopStart).Distinct().Single();
			LoopEnd = encoded.GetAllFormats().Select(x => x.LoopEnd).Distinct().Single();
		}

		public AudioData Export() {
            _audioData.SetLoop(Looping, LoopStart, LoopEnd);
            return _audioData;
        }

        public override string ToString() {
			return base.ToString() + " (VGAudio)";
        }

		void IDisposable.Dispose() { }
    }
}
