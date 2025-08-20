using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class HorizontalDeviceMetricsConverter : TypefaceTableConverter<HorizontalDeviceMetricsTable>
    {
        public override HorizontalDeviceMetricsTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<HorizontalHeaderTable>(out var header)
                || !serializer.TryGet<MaxProfileTable>(out var profile))
            {
                return null;
            }
            var res = new HorizontalDeviceMetricsTable();
            res.Metrics = reader.ReadArray(header.NumberOfHMetrics, () => {
                return new LongHorMetric(reader.ReadUInt16(), reader.ReadInt16());
            });
            res.Bearings = reader.ReadArray(profile.GlyphCount - header.NumberOfHMetrics, reader.ReadInt16);
            return res;
        }

        public override void Write(EndianWriter writer, HorizontalDeviceMetricsTable data)
        {
            throw new NotImplementedException();
        }
    }
}
