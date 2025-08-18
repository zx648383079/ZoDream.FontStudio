using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MergeConverter : TypefaceConverter<MergeTable>
    {
        public override MergeTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return new MergeTable();
        }

        public override void Write(EndianWriter writer, MergeTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
