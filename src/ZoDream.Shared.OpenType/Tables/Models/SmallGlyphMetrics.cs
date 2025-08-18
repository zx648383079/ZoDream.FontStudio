namespace ZoDream.Shared.OpenType.Tables
{
    public struct SmallGlyphMetrics
    {
        public byte height;
        public byte width;
        public sbyte bearingX;
        public sbyte bearingY;
        public byte advance;

        public const int SIZE = 5; //size of SmallGlyphMetrics
    }
}
