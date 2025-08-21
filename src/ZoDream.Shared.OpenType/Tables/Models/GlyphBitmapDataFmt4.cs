using System;
using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt4 : IGlyphBitmapDataFormat
    {
        public int Format => 4;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            var metrics = MetricsConverter.ReadSmall(reader);

            bitmapGlyph.BitmapGlyphAdvanceWidth = metrics.Advance;
            bitmapGlyph.Bounds = new GlyphBound(0, 0, metrics.Width, metrics.Height);
            // 颜色表示 一个字节 表示两个颜色 4 + 4 位
            // 第一个字节 高位为 1 表示重复字节， n < 0x80 (n + 1) 表示重复字节的次数， 第二个字节 重复的颜色
            // 第一个字节 高位为 0 表示文字字节 n < 0x80 表示后面的 n 个字节是原始数据即 Fmt1 格式的位数据
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            throw new NotImplementedException();
        }
    }
}
