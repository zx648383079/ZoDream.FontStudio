using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class AxisVariationsConverter : TypefaceConverter<AxisVariationsTable>
    {
        public override AxisVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            var reserved = reader.ReadUInt16();
            var axisCount = reader.ReadUInt16();
            var res = new AxisVariationsTable();
            res.AxisSegmentMaps = reader.ReadArray(axisCount, () => {
                var positionMapCount = reader.ReadUInt16();
                return reader.ReadArray(positionMapCount, () => {
                    return new AxisValuePair(reader.ReadF2Dot14(), reader.ReadF2Dot14());
                });
            });
            return res;
        }

        public override void Write(EndianWriter writer, AxisVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
