using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class CVTVariationsConverter : TypefaceConverter<CVTVariationsTable>
    {
        public override CVTVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return new CVTVariationsTable();
        }

        public override void Write(EndianWriter writer, CVTVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
