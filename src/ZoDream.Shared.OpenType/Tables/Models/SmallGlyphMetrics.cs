namespace ZoDream.Shared.OpenType.Tables
{
    public struct SmallGlyphMetrics
    {
        public byte Height;
        public byte Width;
        public sbyte BearingX;
        public sbyte BearingY;
        public byte Advance;

        public const int SIZE = 5; //size of SmallGlyphMetrics
    }
}
