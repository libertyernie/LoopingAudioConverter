using System;

namespace BrawlLib.LoopSelection
{
    public unsafe static class Memory
    {
        public static unsafe void Move(IntPtr dst, IntPtr src, uint size)
        {
            byte* from = (byte*)src.ToPointer();
            byte* to = (byte*)dst.ToPointer();
            if (from < to)
                for (uint i = size - 1; i >= 0; i--)
                    to[i] = from[i];
            else if (from > to)
                for (uint i = 0; i < size; i++)
                    to[i] = from[i];
        }

        internal static unsafe void Fill(IntPtr dest, uint length, byte value)
        {
            byte* to = (byte*)dest.ToPointer();
            for (uint i = 0; i < length; i++)
                to[i] = value;
        }
    }
}
