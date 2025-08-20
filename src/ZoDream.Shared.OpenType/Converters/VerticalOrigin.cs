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
            var res = new VerticalOriginTable();
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            res.DefaultVertOriginY = reader.ReadInt16();

            var numVertOriginYMetrics  = reader.ReadUInt16();
            res.VertOriginYMetrics = reader.ReadArray(numVertOriginYMetrics, () => {
                return new VertOriginYMetrics(reader.ReadUInt16(), reader.ReadInt16());
            });
            return res;
        }

        public override void Write(EndianWriter writer, VerticalOriginTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
