using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class EmbeddedBitmapDataConverter : TypefaceConverter<EmbeddedBitmapDataTable>
    {
        public override EmbeddedBitmapDataTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new EmbeddedBitmapDataTable();
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            return res;
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapDataTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
