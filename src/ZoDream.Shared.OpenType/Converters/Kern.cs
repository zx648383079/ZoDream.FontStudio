using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class KernConverter : TypefaceConverter<KernTable>
    {
        public override KernTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new KernTable();
            ushort verion = reader.ReadUInt16();
            ushort nTables = reader.ReadUInt16();
            if (nTables > 1)
            {
                throw new NotSupportedException($"Support for {nTables} kerning tables");
            }

            for (int i = 0; i < nTables; ++i)
            {
                ushort subTableVersion = reader.ReadUInt16();
                ushort len = reader.ReadUInt16(); //Length of the subtable, in bytes (including this header).
                var kerCoverage = new KernCoverage(reader.ReadUInt16());//What type of information is contained in this table.


                switch (kerCoverage.Format)
                {
                    case 0:
                        res.KernSubTables.Add(ReadSubTableFormat0(reader, len - (3 * 2)));//3 header field * 2 byte each
                        break;
                    case 2:
                    //TODO: implement
                    default:
                        break;
                }
            }
            return res;
        }

        private KerningSubTable ReadSubTableFormat0(EndianReader reader, int remainingBytes)
        {
            ushort npairs = reader.ReadUInt16();
            ushort searchRange = reader.ReadUInt16();
            ushort entrySelector = reader.ReadUInt16();
            ushort rangeShift = reader.ReadUInt16();
            //----------------------------------------------  
            var ksubTable = new KerningSubTable(npairs);
            while (npairs > 0)
            {
                ksubTable.AddKernPair(
                    reader.ReadUInt16(), //left//
                    reader.ReadUInt16(),//right
                    reader.ReadInt16());//value 
                npairs--;
            }
            return ksubTable;
        }

        public override void Write(EndianWriter writer, KernTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
