using LoopingAudioConverter.Immutable;
using System;
using System.Collections.Generic;

namespace LoopingAudioConverter.PCM {
	/// <summary>
	/// Represents 16-bit uncompressed PCM data with an arbitary number of channels and an optional loop sequence.
	/// The total sample length of this data is immutable, but the data itself and other properties can be modified.
	/// </summary>
	public class PCM16Audio {
		public PCMData Audio { get; }

		public int Channels => Audio.channels;
		public int SampleRate => Audio.sample_rate;
		public short[] Samples => Audio.samples;

		public LoopType OriginalLoop { get; }
		public LoopType Loop { get; set; }

		public bool Looping => Loop.IsLooping;
		public int LoopStart => Looping ? Loop.LoopStart : 0;
		public int LoopEnd => Looping ? Loop.LoopEnd : Audio.SamplesPerChannel;
		public int LoopLength => LoopEnd - LoopStart;

		public PCM16Audio(PCMData audio, LoopType loop) {
			if (audio.channels > short.MaxValue) throw new ArgumentException("Streams of more than " + short.MaxValue + " channels not supported");
			if (audio.channels <= 0) throw new ArgumentException("Number of channels must be a positive integer");
			if (audio.sample_rate <= 0) throw new ArgumentException("Sample rate must be a positive integer");

			if (loop.IsLooping && loop.LoopEnd > audio.SamplesPerChannel) {
				throw new Exception("The end of the loop is past the end of the file. Double-check the program that generated this data.");
			}

			Audio = audio;
			OriginalLoop = loop;
			Loop = loop;
		}

		/// <summary>
		/// Performs a crude mix down to one channel by averaging all channels equally.
		/// </summary>
		/// <returns>A new PCM16Audio object (or possibly the current object, if already mono)</returns>
		public PCM16Audio MixToMono() {
			if (Channels == 1)
				return this;

			short[] data = new short[Samples.Length / Channels];

			IEnumerable<short> sampleSource = Samples;
			var enumerator = sampleSource.GetEnumerator();

			for (int i = 0; i < data.Length; i++) {
				int sum = 0;
				for (int j = 0; j < Channels ; j++) {
					if (!enumerator.MoveNext())
						break;
					sum += enumerator.Current;
				}
				data[i] = (short)(sum / Channels);
			}

			return new PCM16Audio(new PCMData(1, SampleRate, data), this.Loop);
		}

		/// <summary>
		/// Creates a new non-looping PCM16Audio object containing only the pre-loop portion of this track.
		/// </summary>
		/// <returns>A new PCM16Audio object</returns>
		public PCM16Audio GetPreLoopSegment() {
			short[] data = new short[Channels * LoopStart];
			Array.Copy(Samples, 0, data, 0, data.Length);
			return new PCM16Audio(new PCMData(Channels, SampleRate, data), LoopType.NonLooping);
		}

		/// <summary>
		/// Creates a new looping PCM16Audio object containing only the looping portion of this track.
		/// </summary>
		/// <returns>A new PCM16Audio object</returns>
		public PCM16Audio GetLoopSegment() {
			short[] data = new short[Channels * (LoopEnd - LoopStart)];
			Array.Copy(Samples, Channels * LoopStart, data, 0, data.Length);
			return new PCM16Audio(new PCMData(Channels, SampleRate, data), LoopType.NewLooping(0, data.Length / Channels));
		}

		/// <summary>
		/// Returns an PCM16Audio object that represents this audio when the looping portion is played a given amount of times, with an optional amount of fade-out.
		/// If this is not a looping track, or if the loop count is 1 and the fade is 0 seconds, this object will be returned; otherwise, a new one will be created.
		/// </summary>
		/// <param name="loopCount">Times to play the loop (must be more than 0)</param>
		/// <param name="fadeSec">Amount of time, in seconds, to fade out at the end after the last loop (must be 0 or greater)</param>
		/// <returns>A PCM16Audio object (this or a new one)</returns>
		public PCM16Audio PlayLoopAndFade(int loopCount, decimal fadeSec) {
			if (!Looping) return this;
			if (loopCount == 1 && fadeSec == 0) return this;

			if (loopCount < 1) {
				throw new ArgumentException("Loop count must be at least 1. To play only the portion before the loop, use GetPreLoopSegment.");
			}

			int loopLength = LoopEnd - LoopStart;
			int fadeSamples = Math.Min((int)(SampleRate * fadeSec), LoopLength);
			short[] data = new short[Channels * (LoopStart + loopLength * loopCount + fadeSamples)];

			Array.Copy(this.Samples, 0, data, 0, LoopStart * Channels);
			for (int i = 0; i < loopCount; i++) {
				Array.Copy(Samples, LoopStart * Channels, data, (LoopStart + i * loopLength) * Channels, loopLength * Channels);
			}
			for (int i = 0; i < fadeSamples; i++) {
				double factor = (fadeSamples - i) / (double)fadeSamples;
				for (int j = 0; j < Channels; j++) {
					data[Channels * (this.LoopStart + loopLength * loopCount + i) + j] = (short)(Samples[Channels * (this.LoopStart + i) + j] * factor);
				}
			}

			return new PCM16Audio(new PCMData(this.Channels, this.SampleRate, data), this.Loop);
		}

		public override string ToString() {
			return SampleRate + "Hz " + Channels + " channels: " + Samples.Length + " (" + TimeSpan.FromSeconds(Samples.Length / (SampleRate * Channels)) + ")"
				+ (Looping ? (" loop " + LoopStart + "-" + LoopEnd) : "");
		}
	}
}
