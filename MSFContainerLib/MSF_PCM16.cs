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

        public abstract void SetPCM16Samples(short[] samples);

        public override int LoopStartSample {
            get => Header.loop_start / sizeof(short) / Header.channel_count;
            set => Header.loop_start = value * sizeof(short) * Header.channel_count;
        }
        public override int LoopSampleCount {
            get => Header.loop_length / sizeof(short) / Header.channel_count;
            set => Header.loop_length = value * sizeof(short) * Header.channel_count;
        }
    }
}
