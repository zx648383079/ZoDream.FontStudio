using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT3 : LookupSubTable
    {
        public CoverageTable CoverageTable { get; set; }
        public AlternativeSetTable[] AlternativeSetTables { get; set; }
        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            //Coverage table containing the indices of glyphs with alternative forms(Coverage),
            int iscovered = this.CoverageTable.FindPosition(glyphIndices[pos]);
            return false;
        }
        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
        }
    }
}
