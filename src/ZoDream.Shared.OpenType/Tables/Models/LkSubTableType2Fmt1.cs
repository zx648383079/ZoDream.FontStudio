﻿namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType2Fmt1 : LookupSubTable
    {
        internal PairSetTable[] _pairSetTables;
        public LkSubTableType2Fmt1(PairSetTable[] pairSetTables)
        {
            _pairSetTables = pairSetTables;
        }
        public CoverageTable CoverageTable { get; set; }
        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            //find marker
            var covTable = CoverageTable;
            int lim = inputGlyphs.Count - 1;
            for (int i = 0; i < lim; ++i)
            {
                int firstGlyphFound = covTable.FindPosition(inputGlyphs.GetGlyph(i, out short glyph_advW));
                if (firstGlyphFound > -1)
                {
                    //test this with Palatino A-Y sequence
                    PairSetTable pairSet = _pairSetTables[firstGlyphFound];

                    //check second glyph  
                    ushort second_glyph_index = inputGlyphs.GetGlyph(i + 1, out short second_glyph_w);

                    if (pairSet.FindPairSet(second_glyph_index, out PairSet foundPairSet))
                    {
                        var v1 = foundPairSet.value1;
                        var v2 = foundPairSet.value2;
                        //TODO: recheck for vertical writing ... (YAdvance)
                        if (v1 != null)
                        {
                            inputGlyphs.AppendGlyphOffset(i, v1.XPlacement, v1.YPlacement);
                            inputGlyphs.AppendGlyphAdvance(i, v1.XAdvance, 0);
                        }

                        if (v2 != null)
                        {
                            inputGlyphs.AppendGlyphOffset(i + 1, v2.XPlacement, v2.YPlacement);
                            inputGlyphs.AppendGlyphAdvance(i + 1, v2.XAdvance, 0);
                        }
                    }
                }
            }
        }
    }
}
