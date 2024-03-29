﻿namespace LoopingAudioConverter.MSF
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
    }
}
