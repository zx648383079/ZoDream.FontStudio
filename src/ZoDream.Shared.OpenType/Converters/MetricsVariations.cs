using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MetricsVariationsConverter : TypefaceConverter<MetricsVariationsTable>
    {
        public override MetricsVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new MetricsVariationsTable();
            long startAt = reader.BaseStream.Position;
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            ushort reserved = reader.ReadUInt16();
            ushort valueRecordSize = reader.ReadUInt16();
            ushort valueRecordCount = reader.ReadUInt16();
            ushort itemVariationStoreOffset = reader.ReadUInt16();

            res.ValueRecords = new ValueRecord[valueRecordCount];

            for (int i = 0; i < valueRecordCount; ++i)
            {
                long recStartAt = reader.BaseStream.Position;
                res.ValueRecords[i] = new ValueRecord(
                    reader.ReadString(4),
                    reader.ReadUInt16(),
                    reader.ReadUInt16()
                    );

                reader.BaseStream.Position = recStartAt + valueRecordSize;
            }

            if (valueRecordCount > 0)
            {
                reader.BaseStream.Position = startAt + itemVariationStoreOffset;
                res.ItemVariationStore = HorizontalMetricsVariationsConverter.ReadItemVariationStore(reader);
            }
            return res;
        }


        public override void Write(EndianWriter writer, MetricsVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
