using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public class MSF_Unknown : MSF
    {
        public unsafe MSF_Unknown(MSFHeader header, byte[] body) : base(header, body) { }

        public override int LoopStartSample { get => 0; set => throw new NotImplementedException(); }
        public override int LoopSampleCount { get => 0; set => throw new NotImplementedException(); }

        public unsafe override short[] GetPCM16Samples()
        {
            return new short[0];
        }
    }
}
