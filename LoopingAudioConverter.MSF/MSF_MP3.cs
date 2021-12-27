using LoopingAudioConverter.MP3;
using LoopingAudioConverter.PCM;
using System;

namespace LoopingAudioConverter.MSF
{
    public class MSF_MP3 : MSF
    {
        private readonly MP3Audio MP3;

        public long Bitrate {
            get {
                double sampleCount = MP3.Samples.Length / Header.channel_count;
                double seconds = sampleCount / Header.sample_rate;
                double bytes_per_second = MP3.MP3Data.Length / seconds;
                return (long)Math.Round(bytes_per_second);
            }
        }

        /// <summary>
        /// Creates a new MSF file from an MSF header and MP3 data.
        /// </summary>
        /// <param name="header">The header. Be sure to set the appropriate flags.</param>
        /// <param name="body">The MP3 data.</param>
        public MSF_MP3(MSFHeader header, byte[] body) : base(header, body) {
            if (Header.codec != 7)
                throw new FormatException("The codec in the MSF header is not MP3");

            MP3 = MP3Audio.Read(body);
        }

        /// <summary>
        /// The sample at which the loop starts.
        /// </summary>
        public override int GetLoopStartSample() {
            long x = _header.loop_start;
            x *= Header.sample_rate;
            x *= sizeof(byte);
            x /= Bitrate;
            return checked((int)x);
        }

        /// <summary>
        /// The sample at which the loop ends.
        /// </summary>
        public override int GetLoopSampleCount() {
            long x = _header.loop_length;
            x *= Header.sample_rate;
            x *= sizeof(byte);
            x /= Bitrate;
            return checked((int)x);
        }

        public override PCM16Audio Decode() => MP3;
    }
}
