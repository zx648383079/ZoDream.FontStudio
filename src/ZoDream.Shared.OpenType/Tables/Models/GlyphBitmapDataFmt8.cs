using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

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
            var metrics = MetricsConverter.ReadSmall(reader);
            Pad = reader.ReadByte();
            var numComponents = reader.ReadUInt16();
            Components = reader.ReadArray(numComponents, () => MetricsConverter.ReadComponent(reader));
        }

        public void ReadRawBitmap(EndianReader reader, GlyphData bitmapGlyph, Stream output)
        {
            throw new System.NotImplementedException();
        }
    }
}
