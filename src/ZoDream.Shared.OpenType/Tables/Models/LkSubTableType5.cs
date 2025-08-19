namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType5 : LookupSubTable
    {
        public CoverageTable MarkCoverage { get; set; }
        public CoverageTable LigatureCoverage { get; set; }
        public MarkArrayTable MarkArrayTable { get; set; }
        public LigatureArrayTable LigatureArrayTable { get; set; }
        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            
        }
    }
}
