using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MetaConverter : TypefaceConverter<MetaTable>
    {
        public override MetaTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void Write(EndianWriter writer, MetaTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
