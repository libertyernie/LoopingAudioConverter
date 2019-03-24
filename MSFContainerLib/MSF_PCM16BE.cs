using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public class MSF_PCM16BE : MSF_PCM16
    {
        public unsafe MSF_PCM16BE(MSFHeader header, byte[] body) : base(header, body)
        {
            if (Header.codec != 0)
            {
                throw new FormatException("The codec of this MSF file is not big-endian 16-bit PCM");
            }
        }

        public unsafe override short[] GetPCM16Samples()
        {
            fixed (byte* ptr = Body)
            {
                BigEndianInt16* be_samples = (BigEndianInt16*)ptr;
                short[] samples = new short[Body.Length / sizeof(short)];
                for (int i = 0; i < samples.Length; i++)
                {
                    samples[i] = be_samples[i];
                }
                return samples;
            }
        }

        public unsafe override void SetPCM16Samples(short[] samples)
        {
            byte[] data = new byte[samples.Length * sizeof(short)];
            fixed (byte* ptr = data)
            {
                BigEndianInt16* be_samples = (BigEndianInt16*)ptr;
                for (int i = 0; i < samples.Length; i++)
                {
                    be_samples[i] = samples[i];
                }
            }
            Body = data;
        }
    }
}
