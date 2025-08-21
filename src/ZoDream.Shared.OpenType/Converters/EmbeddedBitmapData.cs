using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class EmbeddedBitmapDataConverter : TypefaceTableConverter<EmbeddedBitmapDataTable>
    {
        public override EmbeddedBitmapDataTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<EmbeddedBitmapLocationTable>(out var location))
            {
                return null;
            }
            var res = new EmbeddedBitmapDataTable();
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            // TODO
            return res;
        }

        public static void ReadBitmap(EndianReader reader, GlyphData glyph)
        {
            reader.Position += glyph.BitmapStreamOffset;
            switch (glyph.BitmapFormat)
            {
                case 1: new GlyphBitmapDataFmt1().FillGlyphInfo(reader, glyph); break;
                case 2: new GlyphBitmapDataFmt2().FillGlyphInfo(reader, glyph); break;
                case 4: new GlyphBitmapDataFmt4().FillGlyphInfo(reader, glyph); break;
                case 5: new GlyphBitmapDataFmt5().FillGlyphInfo(reader, glyph); break;
                case 6: new GlyphBitmapDataFmt6().FillGlyphInfo(reader, glyph); break;
                case 7: new GlyphBitmapDataFmt7().FillGlyphInfo(reader, glyph); break;
                case 8: new GlyphBitmapDataFmt8().FillGlyphInfo(reader, glyph); break;
                case 9: new GlyphBitmapDataFmt9().FillGlyphInfo(reader, glyph); break;
                default:
                    throw new NotSupportedException();
            }
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapDataTable data)
        {
            throw new NotImplementedException();
        }
    }
}
