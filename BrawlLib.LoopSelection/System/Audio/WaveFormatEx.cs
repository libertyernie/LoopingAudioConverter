using System.Runtime.InteropServices;

namespace BrawlLib.LoopSelection
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WaveFormatEx
    {
        public WaveFormatTag wFormatTag;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort wBitsPerSample;
        public ushort cbSize;

        public WaveFormatEx(WaveFormatTag format, int channels, int frequency, int bitsPerSample)
        {
            wFormatTag = format;
            nChannels = (ushort)channels;
            nSamplesPerSec = (uint)frequency;
            nBlockAlign = (ushort)(bitsPerSample * channels / 8);
            nAvgBytesPerSec = nBlockAlign * nSamplesPerSec;
            wBitsPerSample = (ushort)bitsPerSample;
            cbSize = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct WaveFormatExtensible
    {
        public WaveFormatEx Format;
        public ushort wValidBitsPerSample;
        public ushort wSamplesPerBlock;
        public ushort wReserved;
        public uint dwChannelMask;
        public uint SubFormat;
    }
}
