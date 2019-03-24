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
        public unsafe MSF_PCM16LE(MSFHeader header, byte[] body) : base(header, body) {
            if (Header.codec != 1)
            {
                throw new FormatException("The codec of this MSF file is not little-endian 16-bit PCM");
            }
        }

        public unsafe override short[] GetPCM16Samples()
        {
            // TODO: this will not work on big endian processors
            fixed (byte* ptr = Body)
            {
                short* le_samples = (short*)ptr;
                short[] samples = new short[Body.Length / sizeof(short)];
                Marshal.Copy((IntPtr)le_samples, samples, 0, samples.Length);
                return samples;
            }
        }

        public unsafe override void SetPCM16Samples(short[] samples)
        {
            // TODO: this will not work on big endian processors
            byte[] data = new byte[samples.Length * sizeof(short)];
            fixed (byte* ptr = data)
            {
                short* le_samples = (short*)ptr;
                Marshal.Copy(samples, 0, (IntPtr)le_samples, samples.Length);
            }
        }
    }
}
