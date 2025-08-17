using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class FontProgramConverter : TypefaceConverter<FontProgramTable>
    {
        public override FontProgramTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return new FontProgramTable()
            {
                Buffer = reader.ReadAsStream(reader.RemainingLength)
            };
        }

        public override void Write(EndianWriter writer, FontProgramTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
