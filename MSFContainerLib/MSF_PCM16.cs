using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public abstract class MSF_PCM16 : MSF
    {
        public unsafe MSF_PCM16(MSFHeader header, byte[] body) : base(header, body) { }

        public override int LoopStartSample {
            get => Header.loop_start / Header.channel_count / 0x10 * 28;
            set => Header.loop_start = value / 28 * 10 * Header.channel_count;
        }
        public override int LoopEndSample {
            get => Header.loop_length / Header.channel_count / 0x10 * 28;
            set => Header.loop_length = value / 28 * 10 * Header.channel_count;
        }
    }
}
