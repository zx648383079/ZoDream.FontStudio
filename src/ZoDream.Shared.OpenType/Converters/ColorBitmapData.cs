using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ColorBitmapDataConverter : TypefaceConverter<ColorBitmapDataTable>
    {
        public override ColorBitmapDataTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new ColorBitmapDataTable();
            res.Buffer = reader.ReadAsStream(reader.RemainingLength);
            return res;
        }

        public void FillGlyphInfo(ColorBitmapDataTable source, Glyph glyph)
        {

        }

        public void ReadRawBitmap(ColorBitmapDataTable source, Glyph glyph, Stream ouput)
        {

        }

        public override void Write(EndianWriter writer, ColorBitmapDataTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
