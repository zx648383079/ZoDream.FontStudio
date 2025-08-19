using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType7Fmt3 : LookupSubTable
    {
        public CoverageTable[] CoverageTables { get; set; }
        public PosLookupRecord[] PosLookupRecords { get; set; }
        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
        }
    }
}
