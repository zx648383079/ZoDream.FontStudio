using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class EmbeddedBitmapDataConverter : TypefaceTableConverter<EmbeddedBitmapDataTable>
    {
        public override EmbeddedBitmapDataTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<EmbeddedBitmapLocationTable>(out var location))
            {
                return null;
            }
            var res = new EmbeddedBitmapDataTable();
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            // TODO
            return res;
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapDataTable data)
        {
            throw new NotImplementedException();
        }
    }
}
