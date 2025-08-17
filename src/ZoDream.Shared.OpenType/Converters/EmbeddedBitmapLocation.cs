using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class EmbeddedBitmapLocationConverter : TypefaceConverter<EmbeddedBitmapLocationTable>
    {
        public override EmbeddedBitmapLocationTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapLocationTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
