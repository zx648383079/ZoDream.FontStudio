using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class VerticalMetricsConverter : TypefaceTableConverter<VerticalMetricsTable>
    {
        public override VerticalMetricsTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<VerticalHeaderTable>(out var header))
            {
                return null;
            }
            var res = new VerticalMetricsTable();
            res.AdvHeightAndTopSideBearings = reader.ReadArray(header.NumOfLongVerMetrics, () => {
                return new AdvanceHeightAndTopSideBearing(reader.ReadUInt16(),
                    reader.ReadInt16());
            });
            return res;
        }

        public override void Write(EndianWriter writer, VerticalMetricsTable data)
        {
            throw new NotImplementedException();
        }
    }
}
