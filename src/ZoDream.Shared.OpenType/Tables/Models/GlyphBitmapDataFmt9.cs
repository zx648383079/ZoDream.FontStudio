using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphBitmapDataFmt9 : IGlyphBitmapDataFormat
    {
        public int Format => 9;
        public BigGlyphMetrics BigMetrics;
        public EbdtComponent[] Components;

        public void FillGlyphInfo(EndianReader reader, GlyphData bitmapGlyph)
        {
            var metrics = MetricsConverter.ReadBig(reader);
            var numComponents = reader.ReadUInt16();
            Components = reader.ReadArray(numComponents, () => MetricsConverter.ReadComponent(reader));
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            throw new System.NotImplementedException();
        }
    }
}
