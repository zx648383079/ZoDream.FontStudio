namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct IndexSubTableArray
    {
        public readonly ushort FirstGlyphIndex;
        public readonly ushort LastGlyphIndex;
        public readonly uint AdditionalOffsetToIndexSubtable;
        public IndexSubTableArray(ushort firstGlyphIndex, ushort lastGlyphIndex, uint additionalOffsetToIndexSubtable)
        {
            FirstGlyphIndex = firstGlyphIndex;
            LastGlyphIndex = lastGlyphIndex;
            AdditionalOffsetToIndexSubtable = additionalOffsetToIndexSubtable;
        }
    }
}
