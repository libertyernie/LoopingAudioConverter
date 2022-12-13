using LoopingAudioConverter.PCM;
using MP3Sharp;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.MP3 {
	public sealed class MP3Audio : IAudio {
		private readonly byte[] _mp3Data;

		public bool Looping { get; set; }
		public int LoopStart { get; set; }
		public int LoopEnd { get; set; }

		public byte[] MP3Data {
            get {
				byte[] arr = new byte[_mp3Data.Length];
				Array.Copy(_mp3Data, arr, arr.Length);
				return arr;
            }
        }

		public MP3Audio(byte[] mp3data) {
			_mp3Data = mp3data;
		}

		public unsafe PCM16Audio Decode() {
			using (var output = new MemoryStream())
			using (var input = new MemoryStream(MP3Data, false))
			using (var mp3 = new MP3Stream(input)) {
				// TODO figure out why CopyTo is slow
				mp3.CopyTo(output);
				byte[] array = output.ToArray();

				short[] samples = new short[array.Length / sizeof(short)];
				fixed (byte* ptr = array) {
					short* ptr16 = (short*)ptr;
					Marshal.Copy((IntPtr)ptr16, samples, 0, samples.Length);
				}

				return new PCM16Audio(mp3.ChannelCount, mp3.Frequency, samples) {
					Looping = Looping,
					LoopStart = LoopStart,
					LoopEnd = LoopEnd
				};
			}
		}

		public override string ToString() {
			return base.ToString() + " (MP3)";
		}

		void IDisposable.Dispose() { }
	}
}
