namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct VertOriginYMetrics
    {
        public VertOriginYMetrics(ushort glyphIndex, short vertOriginY)
        {
            GlyphIndex = glyphIndex;
            VertOriginY = vertOriginY;
        }

        public ushort GlyphIndex { get; }
        public short VertOriginY { get; }
    }
}
