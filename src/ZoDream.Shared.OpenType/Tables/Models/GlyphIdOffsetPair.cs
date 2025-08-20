namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct GlyphIdOffsetPair
    {
        public readonly ushort GlyphId;
        public readonly ushort Offset;
        public GlyphIdOffsetPair(ushort glyphId, ushort offset)
        {
            GlyphId = glyphId;
            Offset = offset;
        }
    }
}
