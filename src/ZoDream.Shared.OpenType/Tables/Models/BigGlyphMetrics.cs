namespace ZoDream.Shared.OpenType.Tables
{
    public struct BigGlyphMetrics
    {
        public byte Height;
        public byte Width;

        public sbyte HorizontalBearingX;
        public sbyte HorizontalBearingY;
        public byte HorizontalAdvance;

        public sbyte VerticalBearingX;
        public sbyte VerticalBearingY;
        public byte VerticalAdvance;

        public const int SIZE = 8; //size of BigGlyphMetrics
    }
}
