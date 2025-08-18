namespace ZoDream.Shared.OpenType.Tables
{
    public struct BigGlyphMetrics
    {
        public byte height;
        public byte width;

        public sbyte horiBearingX;
        public sbyte horiBearingY;
        public byte horiAdvance;

        public sbyte vertBearingX;
        public sbyte vertBearingY;
        public byte vertAdvance;

        public const int SIZE = 8; //size of BigGlyphMetrics
    }
}
