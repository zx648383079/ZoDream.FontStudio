using System.IO;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt19 : IGlyphBitmapDataFormat
    {
        public int Format => 19;

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
