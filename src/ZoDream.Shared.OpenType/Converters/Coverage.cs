using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class CoverageConverter : TypefaceConverter<CoverageTable>
    {
        public override CoverageTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return Read(reader);
        }

        public static CoverageTable Read(EndianReader reader, long offset)
        {
            reader.Position = offset;
            return Read(reader);
        }

        public static CoverageTable Read(EndianReader reader)
        {
            ushort format = reader.ReadUInt16();
            if (format == 1)
            {
                return new CoverageFmt1()
                {
                    OrderedGlyphIdList = reader.ReadArray(reader.ReadUInt16(), reader.ReadUInt16)
                };
            }
            if (format == 2)
            {
                ushort rangeCount = reader.ReadUInt16();
                ushort[] startIndices = new ushort[rangeCount];
                ushort[] endIndices = new ushort[rangeCount];
                ushort[] coverageIndices = new ushort[rangeCount];
                for (int i = 0; i < rangeCount; ++i)
                {
                    startIndices[i] = reader.ReadUInt16();
                    endIndices[i] = reader.ReadUInt16();
                    coverageIndices[i] = reader.ReadUInt16();
                }

                return new CoverageFmt2()
                {
                    StartIndices = startIndices,
                    EndIndices = endIndices,
                    CoverageIndices = coverageIndices
                };
            }
            throw new NotImplementedException();
        }

        internal static CoverageTable[] ReadMultiple(EndianReader reader, long initPos, ushort[] offsets)
        {
            var results = new CoverageTable[offsets.Length];
            for (int i = 0; i < results.Length; ++i)
            {
                results[i] = Read(reader, initPos + offsets[i]);
            }
            return results;
        }


        public override void Write(EndianWriter writer, CoverageTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        
    }
}
