using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt17 : IGlyphBitmapDataFormat
    {
        public int Format => 17;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            var metrics = MetricsConverter.ReadSmall(reader);

            bitmapGlyph.BitmapGlyphAdvanceWidth = metrics.Advance;
            bitmapGlyph.Bounds = new GlyphBound(0, 0, metrics.Width, metrics.Height);
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            reader.BaseStream.Position += SmallGlyphMetrics.SIZE;
            uint dataLen = reader.ReadUInt32();
            reader.BaseStream.CopyTo(output, dataLen);
        }
    }
}
