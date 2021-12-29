using LoopingAudioConverter.PCM;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
    public class VGAudioAudio : PCM16Audio {
        private readonly AudioData _audioData;

		public VGAudioAudio(AudioData encoded, PCM16Audio decoded) : base(decoded.Channels, decoded.SampleRate, decoded.Samples, decoded.LoopStart, decoded.LoopEnd) {
			_audioData = encoded;
		}

        public AudioData Export() {
            _audioData.SetLoop(Looping, LoopStart, LoopEnd);
            return _audioData;
        }

        public override string ToString() {
			return base.ToString() + " (VGAudio)";
        }
    }
}
