namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct ClassRangeRecord
    {
        //---------------------------------------
        //
        //ClassRangeRecord
        //---------------------------------------
        //Type 	    Name 	            Descriptionc
        //uint16 	Start 	            First glyph ID in the range
        //uint16 	End 	            Last glyph ID in the range
        //uint16 	Class 	            Applied to all glyphs in the range
        //---------------------------------------
        public readonly ushort startGlyphId;
        public readonly ushort endGlyphId;
        public readonly ushort classNo;
        public ClassRangeRecord(ushort startGlyphId, ushort endGlyphId, ushort classNo)
        {
            this.startGlyphId = startGlyphId;
            this.endGlyphId = endGlyphId;
            this.classNo = classNo;
        }
    }
}
