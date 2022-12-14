using LoopingAudioConverter.MP3;
using System;

namespace LoopingAudioConverter.MSF
{
    public class MSF_MP3 : MSF
    {
        public readonly MP3Audio MP3;

        /// <summary>
        /// Creates a new MSF file from an MSF header and MP3 data.
        /// </summary>
        /// <param name="header">The header. Be sure to set the appropriate flags.</param>
        /// <param name="body">The MP3 data.</param>
        public MSF_MP3(MSFHeader header, byte[] body) : base(header, body) {
            if (Header.codec != 7)
                throw new FormatException("The codec in the MSF header is not MP3");

            MP3 = new MP3Audio(body);
        }
    }
}
