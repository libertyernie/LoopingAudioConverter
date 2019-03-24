using BrawlLib.LoopSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public abstract class MSF
    {
        public MSFHeader Header;
        public byte[] Body;

        public abstract short[] GetPCM16Samples();
        public abstract void SetPCM16Samples(short[] samples);

        public abstract int LoopStartSample { get; set; }
        public abstract int LoopEndSample { get; set; }

        protected MSF(MSFHeader header, byte[] body) {
            this.Header = header;
            this.Body = body;
        }

        public unsafe static MSF Parse(byte[] data)
        {
            fixed (byte* ptr = data)
            {
                MSFHeader header = *(MSFHeader*)ptr;
                int size = Math.Min(header.data_size, data.Length - sizeof(MSFHeader));
                byte[] body = new byte[size];
                Marshal.Copy((IntPtr)(ptr + sizeof(MSFHeader)), body, 0, body.Length);
                switch (header.codec)
                {
                    case 0:
                        return new MSF_PCM16BE(header, body);
                    case 1:
                        return new MSF_PCM16LE(header, body);
                    case 7:
                        return MSF_MP3.Decode(header, body);
                    default:
                        return new MSF_Unknown(header, body);
                }
            }
        }
    }
}
