using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT5Fmt1 : LookupSubTable
    {
        public CoverageTable coverageTable;
        public LkSubT5Fmt1_SubRuleSet[] subRuleSets;

        //5.1 Context Substitution Format 1: Simple Glyph Contexts
        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            throw new NotImplementedException();
        }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {

            int coverage_pos = coverageTable.FindPosition(glyphIndices[pos]);
            if (coverage_pos < 0) { return false; }


            return false;
        }
    }
}
