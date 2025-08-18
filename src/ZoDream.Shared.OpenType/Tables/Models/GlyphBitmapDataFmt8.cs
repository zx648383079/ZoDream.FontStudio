namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt8 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 8;

        public SmallGlyphMetrics smallMetrics;
        public byte pad;
        public EbdtComponent[] components;
    }
}
