using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT1Fmt2 : LookupSubTable
    {
        public LkSubTableT1Fmt2(CoverageTable coverageTable, ushort[] substituteGlyphs)
        {
            this.CoverageTable = coverageTable;
            this.SubstituteGlyphs = substituteGlyphs;
        }
        /// <summary>
        /// It provides an array of output glyph indices (Substitute) explicitly matched to the input glyph indices specified in the Coverage table
        /// </summary>
        public ushort[] SubstituteGlyphs { get; }
        public CoverageTable CoverageTable { get; }
        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int foundAt = CoverageTable.FindPosition(glyphIndices[pos]);
            if (foundAt > -1)
            {
                glyphIndices.Replace(pos, SubstituteGlyphs[foundAt]);
                return true;
            }
            return false;
        }

        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            foreach (ushort glyphIndex in CoverageTable.GetExpandedValueIter())
            {
                //2. add substitution glyph
                int foundAt = CoverageTable.FindPosition(glyphIndex);
                outputAssocGlyphs.Add((ushort)(SubstituteGlyphs[foundAt]));
            }
        }
    }
}
