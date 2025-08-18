namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct GlyphIdOffsetPair
    {
        public readonly ushort glyphId;
        public readonly ushort offset;
        public GlyphIdOffsetPair(ushort glyphId, ushort offset)
        {
            this.glyphId = glyphId;
            this.offset = offset;
        }
    }
}
