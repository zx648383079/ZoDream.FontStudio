namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct PairSet
    {
        public readonly ushort secondGlyph;//GlyphID of second glyph in the pair-first glyph is listed in the Coverage table
        public readonly PairValueRecord value1;//Positioning data for the first glyph in the pair
        public readonly PairValueRecord value2;//Positioning data for the second glyph in the pair   
        public PairSet(ushort secondGlyph, PairValueRecord v1, PairValueRecord v2)
        {
            this.secondGlyph = secondGlyph;
            this.value1 = v1;
            this.value2 = v2;
        }
    }
}