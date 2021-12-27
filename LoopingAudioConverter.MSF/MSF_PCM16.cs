using LoopingAudioConverter.PCM;

namespace LoopingAudioConverter.MSF
{
    public abstract class MSF_PCM16 : MSF
    {
        /// <summary>
        /// Creates a new MSF file from an MSF header and PCM data.
        /// </summary>
        /// <param name="header">The header. Be sure to set appropriate values properly.</param>
        /// <param name="body">The raw 16-bit PCM data.</param>
        public unsafe MSF_PCM16(MSFHeader header, byte[] body) : base(header, body) { }

        /// <summary>
        /// Gets the audio data as raw 16-bit PCM (decoding if necessary.)
        /// </summary>
        /// <returns></returns>
        public abstract short[] GetPCM16Samples();

        /// <summary>
        /// The sample at which the loop starts.
        /// </summary>
        public int LoopStartSample {
            get => _header.loop_start / sizeof(short) / Header.channel_count;
            internal set => _header.loop_start = value * sizeof(short) * Header.channel_count;
        }

        /// <summary>
        /// The sample at which the loop ends.
        /// </summary>
        public int LoopSampleCount {
            get => _header.loop_length / sizeof(short) / Header.channel_count;
            internal set => _header.loop_length = value * sizeof(short) * Header.channel_count;
        }

        public override int GetLoopStartSample() => LoopStartSample;
        public override int GetLoopSampleCount() => LoopSampleCount;

        /// <summary>
        /// Decodes the data of the MSF file.
        /// </summary>
        /// <returns>A PCM16Audio object with the header and uncompressed audio data</returns>
        public override PCM16Audio Decode() => new PCM16Audio(
            channels: Header.channel_count,
            sampleRate: Header.sample_rate,
            sample_data: GetPCM16Samples(),
            loop_start: GetLoopStartSample(),
            loop_end: GetLoopSampleCount() - GetLoopStartSample(),
            non_looping: !IsLooping);
    }
}
