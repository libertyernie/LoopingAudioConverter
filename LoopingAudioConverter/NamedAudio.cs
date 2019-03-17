namespace LoopingAudioConverter {
	public class NamedAudio {
		public PCM16Audio Audio { get; private set; }
		public string Name { get; private set; }

		public NamedAudio(PCM16Audio lwav, string name) {
			this.Audio = lwav;
			this.Name = name;
		}

		/// <summary>
		/// Splits a NamedAudio with more than one channel into separate NamedAudios with one channel each.
		/// If this NamedAudio has one channel, it will be returned as-is.
		/// </summary>
		public NamedAudio[] SplitMultiChannelToMono() {
			if (Audio.Channels < 2) return new NamedAudio[] { this };

			NamedAudio[] array = new NamedAudio[Audio.Channels];
			for (int i = 0; i < array.Length; i++) {
				short[] samples = new short[Audio.Samples.Length / Audio.Channels];
				for (int j = 0; j < samples.Length; j++) {
					samples[j] = Audio.Samples[Audio.Channels * j + i];
				}
				array[i] = new NamedAudio(
					new PCM16Audio(1, Audio.SampleRate, samples, Audio.LoopStart, Audio.LoopEnd),
					Name + " (channel " + i + ")"
					);
			}
			return array;
		}

		/// <summary>
		/// Splits a NamedAudio with more than two channels into separate NamedAudios with two or one channels each.
		/// If this NamedAudio has one or two channels, it will be returned as-is.
		/// </summary>
		public NamedAudio[] SplitMultiChannelToStereo() {
			if (Audio.Channels < 3) return new NamedAudio[] { this };

			NamedAudio[] array = new NamedAudio[(Audio.Channels + 1) / 2];
			for (int i = 0; i < array.Length; i++) {
				int leftChannel = i * 2;
				int rightChannel = leftChannel + 1;
				if (Audio.Channels <= rightChannel) {
					// Only one channel left over
					short[] samples = new short[Audio.Samples.Length / Audio.Channels];
					int fromIndex = leftChannel;
					int toIndex = 0;
					while (fromIndex < Audio.Samples.Length) {
						samples[toIndex++] = Audio.Samples[fromIndex++];
						fromIndex += (Audio.Channels - 1);
					}
					array[i] = new NamedAudio(
						new PCM16Audio(1, Audio.SampleRate, samples, Audio.LoopStart, Audio.LoopEnd),
						Name + " (channel " + leftChannel + ")"
						);
				} else {
					// Create stereo track
					short[] samples = new short[2 * Audio.Samples.Length / Audio.Channels];
					int fromIndex = leftChannel;
					int toIndex = 0;
					while (fromIndex < Audio.Samples.Length) {
						samples[toIndex++] = Audio.Samples[fromIndex++];
						samples[toIndex++] = Audio.Samples[fromIndex++];
						fromIndex += (Audio.Channels - 2);
					}
					array[i] = new NamedAudio(
						new PCM16Audio(2, Audio.SampleRate, samples, Audio.LoopStart, Audio.LoopEnd),
						Name + " (channels " + leftChannel + " and " + rightChannel + ")"
						);
				}
			}
			return array;
		}
	}
}
