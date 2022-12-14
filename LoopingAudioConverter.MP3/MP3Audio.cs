using LoopingAudioConverter.PCM;
using MP3Sharp;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.MP3 {
	public sealed class MP3Audio {
		public readonly byte[] Data;

		public MP3Audio(byte[] data) {
			Data = data;
		}

		public unsafe PCM16Audio Decode() {
			using (var output = new MemoryStream())
			using (var input = new MemoryStream(Data, false))
			using (var mp3 = new MP3Stream(input)) {
				mp3.CopyTo(output);
				byte[] array = output.ToArray();

				short[] samples = new short[array.Length / sizeof(short)];
				fixed (byte* ptr = array) {
					short* ptr16 = (short*)ptr;
					Marshal.Copy((IntPtr)ptr16, samples, 0, samples.Length);
				}

				return new PCM16Audio(mp3.ChannelCount, mp3.Frequency, samples);
			}
		}
	}
}
