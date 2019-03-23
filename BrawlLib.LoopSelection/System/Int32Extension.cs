namespace BrawlLib.LoopSelection
{
    public static class intExtension
    {
        public static unsafe int Reverse(this int value)
        {
            return ((value >> 24) & 0xFF) | (value << 24) | ((value >> 8) & 0xFF00) | ((value & 0xFF00) << 8);
        }
        public static int Align(this int value, int align)
        {
            if (align == 0) return value;
            return (value + align - 1) / align * align;
        }
        public static int Clamp(this int value, int min, int max)
        {
            return value <= min ? min : value >= max ? max : value;
        }
        public static int ClampMin(this int value, int min)
        {
            return value <= min ? min : value;
        }
        public static int RoundDownToEven(this int value)
        {
            return value - (value % 2);
        }
        public static int RoundUpToEven(this int value)
        {
            return value + (value % 2);
        }
    }
}
