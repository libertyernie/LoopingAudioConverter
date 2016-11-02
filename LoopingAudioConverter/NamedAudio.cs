namespace LoopingAudioConverter {
    public class NamedAudio {
        public PCM16Audio LWAV { get; private set; }
        public string Name { get; private set; }

        public NamedAudio(PCM16Audio lwav, string name) {
            this.LWAV = lwav;
            this.Name = name;
        }

        /// <summary>
        /// Splits a NamedLWAV with more than one channel into separate NamedLWAVs with one channel each.
        /// If this NamedLWAV has one channel, it will be returned as-is.
        /// </summary>
        public NamedAudio[] SplitMultiChannelToMono() {
            if (LWAV.Channels < 2) return new NamedAudio[] { this };

            NamedAudio[] array = new NamedAudio[LWAV.Channels];
            for (int i = 0; i < array.Length; i++) {
                short[] samples = new short[LWAV.Samples.Length / LWAV.Channels];
                for (int j=0; j<samples.Length; j++) {
                    samples[j] = LWAV.Samples[LWAV.Channels * j + i];
                }
                array[i] = new NamedAudio(
                    new PCM16Audio(1, LWAV.SampleRate, samples, LWAV.LoopStart, LWAV.LoopEnd),
                    Name + " (channel " + i + ")"
                    );
            }
            return array;
        }

        /// <summary>
        /// Splits a NamedLWAV with more than two channels into separate NamedLWAVs with two or one channels each.
        /// If this NamedLWAV has one or two channels, it will be returned as-is.
        /// </summary>
        public NamedAudio[] SplitMultiChannelToStereo() {
            if (LWAV.Channels < 3) return new NamedAudio[] { this };

            NamedAudio[] array = new NamedAudio[(LWAV.Channels + 1) / 2];
            for (int i = 0; i < array.Length; i++) {
                int leftChannel = i * 2;
                int rightChannel = leftChannel + 1;
                if (LWAV.Channels <= rightChannel) {
                    // Only one channel left over
                    short[] samples = new short[LWAV.Samples.Length / LWAV.Channels];
                    int fromIndex = leftChannel;
                    int toIndex = 0;
                    while (fromIndex < LWAV.Samples.Length) {
                        samples[toIndex++] = LWAV.Samples[fromIndex++];
                        fromIndex += (LWAV.Channels - 1);
                    }
                    array[i] = new NamedAudio(
                        new PCM16Audio(1, LWAV.SampleRate, samples, LWAV.LoopStart, LWAV.LoopEnd),
                        Name + " (channel " + leftChannel + ")"
                        );
                } else {
                    // Create stereo track
                    short[] samples = new short[2 * LWAV.Samples.Length / LWAV.Channels];
                    int fromIndex = leftChannel;
                    int toIndex = 0;
                    while (fromIndex < LWAV.Samples.Length) {
                        samples[toIndex++] = LWAV.Samples[fromIndex++];
                        samples[toIndex++] = LWAV.Samples[fromIndex++];
                        fromIndex += (LWAV.Channels - 2);
                    }
                    array[i] = new NamedAudio(
                        new PCM16Audio(2, LWAV.SampleRate, samples, LWAV.LoopStart, LWAV.LoopEnd),
                        Name + " (channels " + leftChannel + " and " + rightChannel + ")"
                        );
                }
            }
            return array;
        }
    }
}
