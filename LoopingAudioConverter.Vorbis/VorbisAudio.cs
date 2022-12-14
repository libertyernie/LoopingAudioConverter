using LoopingAudioConverter.PCM;
using System;
using System.Linq;

namespace LoopingAudioConverter.Vorbis {
	public sealed class VorbisAudio : IAudio {
		private byte[] _originalData;

		public byte[] OggVorbisData {
			get {
				byte[] arr = new byte[_originalData.Length];
				Array.Copy(_originalData, arr, arr.Length);
				return arr;
			}
		}

		public VorbisAudio(byte[] encoded) {
			_originalData = encoded;
		}

		public override string ToString() {
			return base.ToString() + " (Ogg Vorbis)";
		}

		void IDisposable.Dispose() { }
	}
}
