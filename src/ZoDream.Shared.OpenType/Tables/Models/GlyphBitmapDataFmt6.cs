using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt6 : IGlyphBitmapDataFormat
    {
        public int Format => 6;
        public BigGlyphMetrics BigMetrics;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            var metrics = MetricsConverter.ReadBig(reader);

            bitmapGlyph.BitmapGlyphAdvanceWidth = metrics.HorizontalAdvance;
            bitmapGlyph.Bounds = new GlyphBound(0, 0, metrics.Width, metrics.Height);
            // 以二进制 表示图片 1 为黑色 0 白色 从高至低位表示左到右
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            throw new System.NotImplementedException();
        }
    }
}
