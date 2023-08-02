using System;
using System.Collections.Generic;
using System.Linq;

namespace LoopingAudioConverter.PCM {
	public interface ILoopPoints {
		bool Looping { get; }
		int LoopStart { get; }
		int LoopEnd { get; }
	}

	/// <summary>
	/// Represents 16-bit uncompressed PCM data with an arbitary number of channels and an optional loop sequence.
	/// The total sample length of this data is immutable, but the data itself and other properties can be modified.
	/// </summary>
	public sealed class PCM16Audio : ILoopPoints {
		public short Channels { get; }
		public int SampleRate { get; }
		public short[] Samples { get; }

		/// <summary>
		/// Whether the file is known to loop.
		/// </summary>
		public bool Looping { get; set; }

		/// <summary>
		/// The start of the loop, in samples.
		/// </summary>
		public int LoopStart { get; set; }

		/// <summary>
		/// The end of the loop, in samples.
		/// </summary>
		public int LoopEnd { get; set; }

		public int LoopLength => LoopEnd - LoopStart;

		/// <summary>
		/// Creates a WAV with the given metadata and length.
		/// </summary>
		/// <param name="channels">Number of channels</param>
		/// <param name="sampleRate">Sample rate</param>
		/// <param name="sample_data">Audio data (array will not be modified)</param>
		/// <param name="loop_start">Start of loop, in samples (or null for no loop)</param>
		/// <param name="loop_end">End of loop, in samples (or null for end of file); ignored if loop_start is null</param>
		public PCM16Audio(int channels, int sampleRate, short[] sample_data, int? loop_start = null, int? loop_end = null) {
			if (channels > short.MaxValue) throw new ArgumentException("Streams of more than " + short.MaxValue + " channels not supported");
			if (channels <= 0) throw new ArgumentException("Number of channels must be a positive integer");
			if (sampleRate <= 0) throw new ArgumentException("Sample rate must be a positive integer");

			if (loop_start != null && loop_end != null && loop_end.Value > sample_data.Length / channels) {
				throw new Exception("The end of the loop (" + loop_end + " samples) is past the end of the file (" + sample_data.Length / channels + " samples). Double-check the program that generated this data.");
			}

			Channels = (short)channels;
			SampleRate = sampleRate;

			Samples = new short[sample_data.Length];
			Array.Copy(sample_data, Samples, Samples.Length);

			Looping = (loop_start != null);
			LoopStart = loop_start ?? 0;
			LoopEnd = loop_end ?? (Samples.Length / channels);
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

			return new PCM16Audio(1, SampleRate, data, LoopStart, LoopEnd);
		}

		/// <summary>
		/// Creates a new non-looping PCM16Audio object containing only the pre-loop portion of this track.
		/// </summary>
		/// <returns>A new PCM16Audio object</returns>
		public PCM16Audio GetPreLoopSegment() {
			short[] data = new short[Channels * LoopStart];
			Array.Copy(Samples, 0, data, 0, data.Length);
			return new PCM16Audio(Channels, SampleRate, data);
		}

		/// <summary>
		/// Creates a new looping PCM16Audio object containing only the looping portion of this track.
		/// </summary>
		/// <returns>A new PCM16Audio object</returns>
		public PCM16Audio GetLoopSegment() {
			short[] data = new short[Channels * (LoopEnd - LoopStart)];
			Array.Copy(Samples, Channels * LoopStart, data, 0, data.Length);
			return new PCM16Audio(Channels, SampleRate, data, 0);
		}

		/// <summary>
		/// Creates a new non-looping PCM16Audio object containing only the portion of this track after the loop end point. Usually empty (no samples).
		/// </summary>
		/// <returns>A new PCM16Audio object</returns>
		public PCM16Audio GetPostLoopSegment()
		{
			short[] data = new short[Samples.Length - Channels * LoopEnd];
			Array.Copy(Samples, Channels * LoopEnd, data, 0, data.Length);
			return new PCM16Audio(Channels, SampleRate, data, 0);
		}

		/// <summary>
		/// Returns a PCM16Audio object that represents this audio when the looping portion is played a given amount of times, with an optional amount of fade-out.
		/// Any existing loop data will be maintained.
		/// If the file is non-looping, the original file will be returned.
		/// </summary>
		/// <param name="loopCount">Times to play the loop (must be more than 0)</param>
		/// <param name="fadeSec">Amount of time, in seconds, to fade out at the end after the last loop (must be 0 or greater)</param>
		/// <returns>A PCM16Audio object (this or a new one)</returns>
		public PCM16Audio PlayLoopAndFade(int loopCount, decimal fadeSec) {
			if (!Looping) return this;
			if (loopCount == 1 && fadeSec == 0 && this.Samples.Length == this.LoopEnd * this.Channels) return this;

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

			return new PCM16Audio(this.Channels, this.SampleRate, data, this.LoopStart, this.LoopEnd);
		}

		/// <summary>
		/// Returns an PCM16Audio object that represents this audio when played for a given amount of time, with an optional amount of fade-out.
		/// Loop data will not be maintained in the output file.
		/// If the file is already non-looping, the original file will be returned.
		/// </summary>
		/// <param name="duration">Amount of time to play, in seconds</param>
		/// <param name="fadeSec">Amount of time, in seconds, to fade out at the end after the last loop (must be 0 or greater)</param>
		/// <returns>A PCM16Audio object (this or a new one)</returns>
		public PCM16Audio PlayAndFade(decimal duration, decimal fadeSec) {
			if (!Looping) return this;

			if (duration <= 0) {
				throw new ArgumentException("Duration must be greater than 0.");
			}

			int playSamples = (int)(SampleRate * duration);
			int fadeSamples = Math.Min((int)(SampleRate * fadeSec), LoopLength);

			IEnumerable<short> getSamples() {
				int pos = 0;
				int fadePos = playSamples;
				int endPos = playSamples + fadeSamples;

				for (int k = 0; k < LoopEnd; k++) {
					for (int c = 0; c < Channels; c++) {
						yield return this.Samples[k * Channels + c];
					}
					pos++;
				}
				while (true) {
					for (int k = LoopStart; k < LoopEnd; k++) {
						for (int c = 0; c < Channels; c++) {
							if (pos >= fadePos) {
								double factor = 1.0 - (pos - fadePos) / (double)(endPos - fadePos);
								yield return (short)(this.Samples[k * Channels + c] * factor);
							} else {
								yield return this.Samples[k * Channels + c];
							}
						}
						pos++;
						if (pos >= endPos) yield break;
					}
				}
			}

			return new PCM16Audio(this.Channels, this.SampleRate, getSamples().ToArray());
		}

		public override string ToString() {
			return SampleRate + "Hz " + Channels + " channels: " + Samples.Length + " (" + TimeSpan.FromSeconds(Samples.Length / (SampleRate * Channels)) + ")"
				+ (Looping ? (" loop " + LoopStart + "-" + LoopEnd) : "");
		}
	}
}
