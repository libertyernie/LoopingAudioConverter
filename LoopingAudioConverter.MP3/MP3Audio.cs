using LoopingAudioConverter.Immutable;
using LoopingAudioConverter.PCM;
using MP3Sharp;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.MP3 {
	/// <summary>
	/// Represents 16-bit uncompressed PCM data sourced from an original MP3 rendition.
	/// </summary>
	public class MP3Audio : PCM16Audio {
		private readonly byte[] _mp3Data;

		public byte[] MP3Data {
            get {
				byte[] arr = new byte[_mp3Data.Length];
				Array.Copy(_mp3Data, arr, arr.Length);
				return arr;
            }
        }

		private MP3Audio(byte[] mp3data, PCMData audio) : base(audio, LoopType.NonLooping) {
			_mp3Data = mp3data;
		}

		public unsafe static MP3Audio Read(byte[] mp3data) {
			using (var output = new MemoryStream())
			using (var input = new MemoryStream(mp3data, false))
			using (var mp3 = new MP3Stream(input)) {
				// TODO figure out why CopyTo is slow
				mp3.CopyTo(output);
				byte[] array = output.ToArray();

				short[] samples = new short[array.Length / sizeof(short)];
				fixed (byte* ptr = array) {
					short* ptr16 = (short*)ptr;
					Marshal.Copy((IntPtr)ptr16, samples, 0, samples.Length);
				}

				return new MP3Audio(mp3data, new PCMData(mp3.ChannelCount, mp3.Frequency, samples));
			}
		}

		public override string ToString() {
			return base.ToString() + " (MP3)";
		}
	}
}
