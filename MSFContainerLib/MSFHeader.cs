using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BigEndianInt16
    {
        private byte b1;
        private byte b2;

        public static implicit operator short(BigEndianInt16 val) =>
            (short)(val.b1 << 8 | val.b2);
        public static implicit operator BigEndianInt16(short val) => new BigEndianInt16
        {
            b1 = (byte)(val >> 8),
            b2 = (byte)val
        };

        public int Value => this;
        public override string ToString() => $"{Value} ({b1} {b2})";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BigEndianInt32
    {
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;

        public static implicit operator int(BigEndianInt32 val) =>
            val.b1 << 24
            | val.b2 << 16
            | val.b3 << 8
            | val.b4;
        public static implicit operator BigEndianInt32(int val) => new BigEndianInt32
        {
            b1 = (byte)(val >> 24),
            b2 = (byte)(val >> 16),
            b3 = (byte)(val >> 8),
            b4 = (byte)val
        };

        public int Value => this;
        public override string ToString() => $"{Value} ({b1} {b2} {b3} {b4})";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSFTag
    {
        public byte m;
        public byte s;
        public byte f;
        public byte version;

        public static MSFTag Create(byte version = 0x43) => new MSFTag
        {
            m = 0x4D,
            s = 0x53,
            f = 0x46,
            version = version
        };

        public override string ToString() => $"{(char)m}{(char)s}{(char)f} version 0x{((int)version).ToString("X2")}";
    }

    [Flags]
    public enum MSFFlag : int
    {
        LoopMarker0 = 0x01,
        LoopMarker1 = 0x02,
        LoopMarker2 = 0x04,
        LoopMarker3 = 0x08,
        Resample = 0x10,
        MP3VBR = 0x20,
        MP3JointStereo = 0x40
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MSFFlags
    {
        public BigEndianInt32 val;

        public MSFFlag Flags {
            get {
                return (MSFFlag)(int)val;
            }
            set {
                val = (int)value;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MSFHeader
    {
        public MSFTag tag;
        public BigEndianInt32 codec;
        public BigEndianInt32 channel_count;
        public BigEndianInt32 data_size;
        public BigEndianInt32 sample_rate;
        public MSFFlags flags;
        public BigEndianInt32 loop_start;
        public BigEndianInt32 loop_length;

        public fixed byte padding[32];

        public static unsafe MSFHeader Create()
        {
            MSFHeader h = new MSFHeader
            {
                tag = MSFTag.Create(),
                flags = new MSFFlags { Flags = MSFFlag.Resample }
            };
            for (int i = 0; i < 32; i++)
                h.padding[i] = 0xFF;
            return h;
        }
    }
}
