using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt18 : IGlyphBitmapDataFormat
    {
        public int Format => 18;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            var metrics = MetricsConverter.ReadBig(reader);

            bitmapGlyph.BitmapGlyphAdvanceWidth = metrics.HorizontalAdvance;
            bitmapGlyph.Bounds = new GlyphBound(0, 0, metrics.Width, metrics.Height);
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            reader.BaseStream.Position += BigGlyphMetrics.SIZE;
            uint dataLen = reader.ReadUInt32();
            reader.BaseStream.CopyTo(output, dataLen);
        }
    }
}
