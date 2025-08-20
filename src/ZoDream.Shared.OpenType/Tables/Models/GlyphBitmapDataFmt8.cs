using System.IO;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt8 : IGlyphBitmapDataFormat
    {
        public int Format => 8;

        public SmallGlyphMetrics SmallMetrics;
        public byte Pad;
        public EbdtComponent[] Components;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            throw new System.NotImplementedException();
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            throw new System.NotImplementedException();
        }
    }
}
