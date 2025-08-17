using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ControlValueConverter : TypefaceConverter<ControlValueTable>
    {
        public override ControlValueTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new ControlValueTable();
            res.ControlValues = reader.ReadArray((int)(reader.RemainingLength / sizeof(short)), reader.ReadInt16);
            return res;
        }

        public override void Write(EndianWriter writer, ControlValueTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
