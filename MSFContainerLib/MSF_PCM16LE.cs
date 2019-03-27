using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public class MSF_PCM16LE : MSF_PCM16
    {
        /// <summary>
        /// Creates a new MSF file from an MSF header and PCM data.
        /// </summary>
        /// <param name="header">The header. Be sure to set appropriate values properly.</param>
        /// <param name="body">The raw 16-bit PCM data.</param>
        public unsafe MSF_PCM16LE(MSFHeader header, byte[] body) : base(header, body) {
            if (Header.codec != 1)
            {
                throw new FormatException("The codec of this MSF file is not little-endian 16-bit PCM");
            }
        }

        /// <summary>
        /// Gets the audio data as raw 16-bit PCM.
        /// </summary>
        /// <returns></returns>
        public unsafe override short[] GetPCM16Samples()
        {
            fixed (byte* ptr = Body)
            {
                LittleEndianInt16* be_samples = (LittleEndianInt16*)ptr;
                short[] samples = new short[Body.Length / sizeof(short)];
                for (int i = 0; i < samples.Length; i++)
                {
                    samples[i] = be_samples[i];
                }
                return samples;
            }
        }
    }
}
