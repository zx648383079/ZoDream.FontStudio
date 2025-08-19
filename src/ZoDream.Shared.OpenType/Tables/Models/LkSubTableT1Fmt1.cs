using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT1Fmt1 : LookupSubTable
    {
        public LkSubTableT1Fmt1(CoverageTable coverageTable, ushort deltaGlyph)
        {
            this.CoverageTable = coverageTable;
            this.DeltaGlyph = deltaGlyph;
        }
        /// <summary>
        /// Add to original GlyphID to get substitute GlyphID
        /// </summary>
        public ushort DeltaGlyph { get; private set; }
        public CoverageTable CoverageTable { get; private set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            ushort glyphIndex = glyphIndices[pos];
            if (CoverageTable.FindPosition(glyphIndex) > -1)
            {
                glyphIndices.Replace(pos, (ushort)(glyphIndex + DeltaGlyph));
                return true;
            }
            return false;
        }
        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            //1. iterate glyphs from CoverageTable                    
            foreach (ushort glyphIndex in CoverageTable.GetExpandedValueIter())
            {
                //2. add substitution glyph
                outputAssocGlyphs.Add((ushort)(glyphIndex + DeltaGlyph));
            }
        }
    }
}
