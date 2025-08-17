using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class VerticalDeviceMetricsConverter : TypefaceConverter<VerticalDeviceMetricsTable>
    {
        public override VerticalDeviceMetricsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new VerticalDeviceMetricsTable();
            var version = reader.ReadUInt16();
            var numRecs = reader.ReadUInt16();
            var numRatios = reader.ReadUInt16();
            res.Ratios = reader.ReadArray(numRatios, () => {
                return new Ratio(reader.ReadByte(),
                    reader.ReadByte(),
                    reader.ReadByte(),
                    reader.ReadByte());
            });
            var offsets = reader.ReadArray(numRatios, reader.ReadUInt16);
            return res;
        }

        public override void Write(EndianWriter writer, VerticalDeviceMetricsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
