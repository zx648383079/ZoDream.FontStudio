namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable5 : IndexSubTableBase
    {
        public override int SubTypeNo => 5;
        public uint imageSize;
        public BigGlyphMetrics BigGlyphMetrics = new BigGlyphMetrics();

        public ushort[] glyphIdArray;
    }
}
