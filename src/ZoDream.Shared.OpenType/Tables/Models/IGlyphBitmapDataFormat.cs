using System.IO;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType.Tables
{
    public interface IGlyphBitmapDataFormat
    {
        public int Format { get; }

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph);
        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output);
    }
}
