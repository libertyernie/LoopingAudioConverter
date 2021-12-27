using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter.MSF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct bshort
    {
        private byte b1;
        private byte b2;

        public static implicit operator short(bshort val) =>
            (short)(val.b1 << 8 | val.b2);
        public static implicit operator bshort(short val) => new bshort
        {
            b1 = (byte)(val >> 8),
            b2 = (byte)val
        };

        public int Value => this;
        public override string ToString() => $"{Value}";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct bint
    {
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;

        public static implicit operator int(bint val) =>
            val.b1 << 24
            | val.b2 << 16
            | val.b3 << 8
            | val.b4;
        public static implicit operator bint(int val) => new bint
        {
            b1 = (byte)(val >> 24),
            b2 = (byte)(val >> 16),
            b3 = (byte)(val >> 8),
            b4 = (byte)val
        };

        public int Value => this;
        public override string ToString() => $"{Value}";
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
        public bint val;

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
        public bint codec;
        public bint channel_count;
        public bint data_size;
        public bint sample_rate;
        public MSFFlags flags;
        public bint loop_start;
        public bint loop_length;

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
