using LoopingAudioConverter.PCM;
using System;

namespace LoopingAudioConverter.MSF
{
    public class MSF_PCM16BE : MSF_PCM16
    {
        /// <summary>
        /// Creates a new MSF file from an MSF header and PCM data.
        /// </summary>
        /// <param name="header">The header. Be sure to set appropriate values properly.</param>
        /// <param name="body">The raw 16-bit PCM data.</param>
        public unsafe MSF_PCM16BE(MSFHeader header, byte[] body) : base(header, body)
        {
            if (Header.codec != 0)
            {
                throw new FormatException("The codec of this MSF file is not big-endian 16-bit PCM");
            }
        }

        /// <summary>
        /// Creates a new MSF (with a 16-bit PCM codec) using a PCM16Audio object as the source.
        /// </summary>
        /// <param name="source">The IPCM16AudioSource object</param>
        /// <returns></returns>
        public unsafe static MSF_PCM16BE FromPCM(PCM16Audio source) {
            short[] samples = source.Samples;

            MSFHeader header = MSFHeader.Create();
            header.codec = 0;
            header.channel_count = source.Channels;
            header.data_size = samples.Length * sizeof(short);
            header.sample_rate = source.SampleRate;
            if (source.Looping)
                header.flags.Flags |= MSFFlag.LoopMarker0;

            byte[] data = new byte[samples.Length * sizeof(short)];
            fixed (byte* bptr = data) {
                bshort* dest = (bshort*)bptr;
                for (int i = 0; i < samples.Length; i++) {
                    dest[i] = samples[i];
                }
            }
            MSF_PCM16BE msf = new MSF_PCM16BE(header, data);

            if (source.Looping) {
                msf.LoopStartSample = source.LoopStart;
                msf.LoopSampleCount = source.LoopEnd - source.LoopStart;
            }

            return msf;
        }
    }
}
