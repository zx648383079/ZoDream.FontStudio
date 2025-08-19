using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT6Fmt2 : LookupSubTable
    {
        public CoverageTable CoverageTable { get; set; }
        public ClassDefTable BacktrackClassDef { get; set; }
        public ClassDefTable InputClassDef { get; set; }
        public ClassDefTable LookaheadClassDef { get; set; }
        public ChainSubClassSet[] ChainSubClassSets { get; set; }
        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {

            int coverage_pos = CoverageTable.FindPosition(glyphIndices[pos]);
            if (coverage_pos < 0) { return false; }

            //--

            int inputClass = InputClassDef.GetClassValue(glyphIndices[pos]);


            return false;
        }
        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
        }
    }
}
