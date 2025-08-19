using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType8Fmt1 : LookupSubTable
    {
        public CoverageTable CoverageTable { get; set; }
        public PosRuleSetTable[] PosRuleSetTables { get; set; }
        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            
        }
    }
}
