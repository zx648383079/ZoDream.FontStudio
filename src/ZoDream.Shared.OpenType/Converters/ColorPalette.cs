using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ColorPaletteConverter : TypefaceConverter<ColorPaletteTable>
    {
        public override ColorPaletteTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void Write(EndianWriter writer, ColorPaletteTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
