using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class VerticalMetricsVariationsConverter : TypefaceConverter<VerticalMetricsVariationsTable>
    {
        public override VerticalMetricsVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new VerticalMetricsVariationsTable();
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            res.ItemVariationStoreOffset = reader.ReadUInt32();
            res.AdvanceHeightMappingOffset = reader.ReadUInt32();
            res.TsbMappingOffset = reader.ReadUInt32();
            res.BsbMappingOffset = reader.ReadUInt32();
            res.VOrgMappingOffset = reader.ReadUInt32();
            return res;
        }

        public override void Write(EndianWriter writer, VerticalMetricsVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
