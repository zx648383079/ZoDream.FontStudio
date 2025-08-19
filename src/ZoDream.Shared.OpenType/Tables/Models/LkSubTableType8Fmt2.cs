using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType8Fmt2 : LookupSubTable
    {
        public CoverageTable CoverageTable { get; set; }
        public PosClassSetTable[] PosClassSetTables { get; set; }

        public ClassDefTable BackTrackClassDef { get; set; }
        public ClassDefTable InputClassDef { get; set; }
        public ClassDefTable LookaheadClassDef { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            ushort glyphIndex = inputGlyphs.GetGlyph(startAt, out short advW);

            int coverage_pos = CoverageTable.FindPosition(glyphIndex);
            if (coverage_pos < 0) { return; }

        }
    }
}
