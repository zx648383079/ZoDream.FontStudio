using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ControlValueProgramConverter : TypefaceConverter<ControlValueProgramTable>
    {
        public override ControlValueProgramTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return new ControlValueProgramTable()
            {
                Buffer = reader.ReadAsStream(reader.RemainingLength)
            };
        }

        public override void Write(EndianWriter writer, ControlValueProgramTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
