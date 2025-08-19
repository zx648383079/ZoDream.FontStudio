using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT6Fmt1 : LookupSubTable
    {
        public CoverageTable CoverageTable { get; set; }
        public ChainSubRuleSetTable[] SubRuleSets { get; set; }
        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            return false;
        }
        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
        }
    }
}
