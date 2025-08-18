namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable2 : IndexSubTableBase
    {
        public override int SubTypeNo => 2;
        public uint imageSize;
        public BigGlyphMetrics BigGlyphMetrics = new BigGlyphMetrics();
    }
}
