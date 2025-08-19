using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class HorizontalMetricsVariationsConverter : TypefaceConverter<HorizontalMetricsVariationsTable>
    {
        public override HorizontalMetricsVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new HorizontalMetricsVariationsTable();
            long beginAt = reader.BaseStream.Position;
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            uint itemVariationStoreOffset = reader.ReadUInt32();
            uint advanceWidthMappingOffset = reader.ReadUInt32();
            uint lsbMappingOffset = reader.ReadUInt32();
            uint rsbMappingOffset = reader.ReadUInt32();
            //            

            //itemVariationStore
            reader.BaseStream.Position = beginAt + itemVariationStoreOffset;
            res._itemVartionStore = ReadItemVariationStore(reader);

            //-----------------------------------------
            if (advanceWidthMappingOffset > 0)
            {
                reader.BaseStream.Position = beginAt + advanceWidthMappingOffset;
                res._advanceWidthMapping = ReadDeltaSetIndexMap(reader);
            }
            if (lsbMappingOffset > 0)
            {
                reader.BaseStream.Position = beginAt + lsbMappingOffset;
                res._lsbMapping = ReadDeltaSetIndexMap(reader);
            }
            if (rsbMappingOffset > 0)
            {
                reader.BaseStream.Position = beginAt + rsbMappingOffset;
                res._rsbMapping = ReadDeltaSetIndexMap(reader);
            }
            return res;
        }

        private DeltaSetIndexMap[] ReadDeltaSetIndexMap(EndianReader reader)
        {
            ushort entryFormat = reader.ReadUInt16();
            ushort mapCount = reader.ReadUInt16();
            int entrySize = ((entryFormat & DeltaSetIndexMap.MAP_ENTRY_SIZE_MASK) >> DeltaSetIndexMap.MAP_ENTRY_SIZE_SHIFT) + 1;
            int innerIndexEntryMask = (1 << ((entryFormat & DeltaSetIndexMap.INNER_INDEX_BIT_COUNT_MASK) + 1)) - 1;
            int outerIndexEntryShift = (entryFormat & DeltaSetIndexMap.INNER_INDEX_BIT_COUNT_MASK) + 1;

            int mapDataSize = mapCount * entrySize;

            DeltaSetIndexMap[] deltaSetIndexMaps = new DeltaSetIndexMap[mapCount];

            for (int i = 0; i < mapCount; ++i)
            {
                int entry;
                switch (entrySize)
                {
                    default: throw new NotSupportedException();
                    case 1: entry = reader.ReadByte(); break;
                    case 2: entry = (reader.ReadByte() << 8) | reader.ReadByte(); break;
                    case 3: entry = (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte(); break;
                    case 4: entry = (reader.ReadByte() << 24) | (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte(); break;
                }
                //***
                deltaSetIndexMaps[i] = new DeltaSetIndexMap((ushort)(entry & innerIndexEntryMask), (ushort)(entry >> outerIndexEntryShift));
            }

            return deltaSetIndexMaps;
        }

        internal static ItemVariationStoreTable ReadItemVariationStore(EndianReader reader)
        {
            var res = new ItemVariationStoreTable();
            ushort axisCount = reader.ReadUInt16();
            ushort regionCount = reader.ReadUInt16();
            res.variationRegions = new VariationRegion[regionCount];
            for (int i = 0; i < regionCount; ++i)
            {
                var variationRegion = ReadVariationRegion(reader, axisCount);
                res.variationRegions[i] = variationRegion;
            }
            return res;
        }

        private static VariationRegion ReadVariationRegion(EndianReader reader, ushort axisCount)
        {
            var res = new VariationRegion();
            res.regionAxes = new RegionAxisCoordinate[axisCount];
            for (int i = 0; i < axisCount; ++i)
            {
                res.regionAxes[i] = new RegionAxisCoordinate(
                    reader.ReadF2Dot14(), //start
                    reader.ReadF2Dot14(), //peak
                    reader.ReadF2Dot14() //end
                    );
            }
            return res;
        }

        public override void Write(EndianWriter writer, HorizontalMetricsVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
