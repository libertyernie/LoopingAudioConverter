using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	/// <summary>
	/// Represents 16-bit uncompressed PCM data with an arbitary number of channels and an optional loop sequence.
	/// The total sample length of this data is immutable, but the data itself and other properties can be modified.
	/// </summary>
    public class LWAV {
		public short Channels { get; private set; }
		public int SampleRate { get; private set; }
		public short[] Samples { get; private set; }

		public bool Looping { get; private set; }
		public int LoopStart { get; private set; }
		public int LoopEnd { get; private set; }

        /// <summary>
        /// Creates a WAV with the given metadata and length.
        /// </summary>
        /// <param name="channels">Number of channels</param>
        /// <param name="sampleRate">Sample rate</param>
		/// <param name="sample_data">Audio data (array will not be modified)</param>
		/// <param name="loop_start">Start of loop, in samples (or null for no loop)</param>
		/// <param name="loop_end">End of loop, in samples (or null for end of file); ignored if loop_start is null</param>
		public unsafe LWAV(int channels, int sampleRate, short[] sample_data, int? loop_start = null, int? loop_end = null) {
			if (channels > short.MaxValue) throw new ArgumentException("Streams of more than " + short.MaxValue + " channels not supported");
			if (channels <= 0) throw new ArgumentException("Number of channels must be a positive integer");
			if (sampleRate <= 0) throw new ArgumentException("Sample rate must be a positive integer");

			Channels = (short)channels;
			SampleRate = sampleRate;

			Samples = new short[sample_data.Length];
			Array.Copy(sample_data, Samples, Samples.Length);

			Looping = (loop_start != null);
			LoopStart = loop_start ?? 0;
			LoopEnd = loop_end ?? Samples.Length;
        }

        public override string ToString() {
            return SampleRate + "Hz " + Channels + " channels: " + Samples.Length + " (" + TimeSpan.FromSeconds(Samples.Length / (SampleRate * Channels)) + ")"
                + (Looping ? (" loop " + LoopStart + "-" + LoopEnd) : "");
        }
    }
}
