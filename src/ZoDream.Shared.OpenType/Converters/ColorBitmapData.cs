using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ColorBitmapDataConverter : TypefaceTableConverter<ColorBitmapDataTable>
    {
        public override ColorBitmapDataTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<ColorBitmapLocationTable>(out var location))
            {
                return null;
            }
            var res = new ColorBitmapDataTable();
            res.Buffer = reader.ReadAsStream(reader.RemainingLength);
            return res;
        }

        public static void ReadBitmap(EndianReader reader, GlyphData glyph)
        {
            reader.Position += glyph.BitmapStreamOffset;
            switch (glyph.BitmapFormat)
            {
                case 17: new GlyphBitmapDataFmt17().FillGlyphInfo(reader, glyph); break;
                case 18: new GlyphBitmapDataFmt18().FillGlyphInfo(reader, glyph); break;
                case 19: new GlyphBitmapDataFmt19().FillGlyphInfo(reader, glyph); break;
                default:
                    throw new NotSupportedException();
            }
        }



        public override void Write(EndianWriter writer, ColorBitmapDataTable data)
        {
            throw new NotImplementedException();
        }
    }
}
