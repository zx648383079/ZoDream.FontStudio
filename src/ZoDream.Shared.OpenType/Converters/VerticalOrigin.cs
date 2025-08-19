using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class VerticalOriginConverter : TypefaceConverter<VerticalOriginTable>
    {
        public override VerticalOriginTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            // TODO
            return new VerticalOriginTable();
        }

        public override void Write(EndianWriter writer, VerticalOriginTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
