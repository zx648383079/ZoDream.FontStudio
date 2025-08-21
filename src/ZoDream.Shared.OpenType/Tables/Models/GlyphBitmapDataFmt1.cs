using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt1 : IGlyphBitmapDataFormat
    {
        public int Format => 1;
        public SmallGlyphMetrics smallGlyphMetrics;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            var metrics = MetricsConverter.ReadSmall(reader);

            bitmapGlyph.BitmapGlyphAdvanceWidth = metrics.Advance;
            bitmapGlyph.Bounds = new GlyphBound(0, 0, metrics.Width, metrics.Height);
            // 一个字节表示 8 个颜色
            // 以二进制 表示图片 1 为黑色 0 白色 从高至低位表示左到右
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            throw new System.NotImplementedException();
        }
    }
}
