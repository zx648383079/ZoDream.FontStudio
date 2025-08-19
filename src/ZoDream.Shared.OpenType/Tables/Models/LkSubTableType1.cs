using System;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType1 : LookupSubTable
    {
        public LkSubTableType1(CoverageTable coverage, PairValueRecord singleValue)
        {
            this.Format = 1;
            _coverageTable = coverage;
            _valueRecords = new PairValueRecord[] { singleValue };
        }

        public LkSubTableType1(CoverageTable coverage, PairValueRecord[] valueRecords)
        {
            this.Format = 2;
            _coverageTable = coverage;
            _valueRecords = valueRecords;
        }

        public int Format { get; }
        readonly CoverageTable _coverageTable;
        readonly PairValueRecord[] _valueRecords;

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            int lim = Math.Min(startAt + len, inputGlyphs.Count);
            for (int i = startAt; i < lim; ++i)
            {
                ushort glyph_index = inputGlyphs.GetGlyph(i, out short glyph_advW);
                int cov_index = _coverageTable.FindPosition(glyph_index);
                if (cov_index > -1)
                {
                    var vr = _valueRecords[Format == 1 ? 0 : cov_index];
                    inputGlyphs.AppendGlyphOffset(i, vr.XPlacement, vr.YPlacement);
                    inputGlyphs.AppendGlyphAdvance(i, vr.XAdvance, 0);
                }
            }
        }
    }
}
