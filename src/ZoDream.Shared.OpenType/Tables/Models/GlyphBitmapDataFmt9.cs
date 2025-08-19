namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt9 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 9;
        public BigGlyphMetrics bigMetrics;
        public EbdtComponent[] components;
    }
}
