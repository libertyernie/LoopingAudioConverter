using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.Linq;
using System.Threading.Tasks;
using VGAudio.Containers.Wave;
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

		public Task<PCM16Audio> DecodeAsync() {
			byte[] wavedata = new WaveWriter().GetFile(_audioData);
			return Task.FromResult(WaveConverter.FromByteArray(wavedata));
		}

		public override string ToString() {
			return base.ToString() + " (VGAudio)";
        }

		void IDisposable.Dispose() { }
	}
}
