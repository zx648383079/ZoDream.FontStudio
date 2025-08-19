using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class StandardBitmapGraphicsConverter : TypefaceConverter<StandardBitmapGraphicsTable>
    {
        public override StandardBitmapGraphicsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            // TODO
            return new StandardBitmapGraphicsTable();
        }

        public override void Write(EndianWriter writer, StandardBitmapGraphicsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
