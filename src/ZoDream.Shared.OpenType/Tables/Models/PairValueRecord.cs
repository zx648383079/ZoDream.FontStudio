namespace ZoDream.Shared.OpenType.Tables
{
    public class PairValueRecord
    {
        internal const int FMT_XPlacement = 1;
        internal const int FMT_YPlacement = 1 << 1;
        internal const int FMT_XAdvance = 1 << 2;
        internal const int FMT_YAdvance = 1 << 3;
        internal const int FMT_XPlaDevice = 1 << 4;
        internal const int FMT_YPlaDevice = 1 << 5;
        internal const int FMT_XAdvDevice = 1 << 6;
        internal const int FMT_YAdvDevice = 1 << 7;

        public short XPlacement;
        public short YPlacement;
        public short XAdvance;
        public short YAdvance;
        public ushort XPlaDevice;
        public ushort YPlaDevice;
        public ushort XAdvDevice;
        public ushort YAdvDevice;

        public ushort valueFormat;

    }
}