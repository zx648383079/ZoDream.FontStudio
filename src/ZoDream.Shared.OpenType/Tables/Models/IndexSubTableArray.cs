namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct IndexSubTableArray
    {
        public readonly ushort FirstGlyphIndex;
        public readonly ushort LastGlyphIndex;
        public readonly uint additionalOffsetToIndexSubtable;
        public IndexSubTableArray(ushort firstGlyphIndex, ushort lastGlyphIndex, uint additionalOffsetToIndexSubtable)
        {
            this.FirstGlyphIndex = firstGlyphIndex;
            this.LastGlyphIndex = lastGlyphIndex;
            this.additionalOffsetToIndexSubtable = additionalOffsetToIndexSubtable;
        }
    }
}
