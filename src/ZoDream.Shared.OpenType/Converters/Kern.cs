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
                        res.SubTables.Add(ReadSubTableFormat0(reader, len - (3 * 2)));//3 header field * 2 byte each
                        break;
                    case 2:
                        res.SubTables.Add(ReadSubTableFormat2(reader, len - (3 * 2)));//3 header field * 2 byte each
                        break;
                    default:
                        break;
                }
            }
            return res;
        }

        private KernSubtableFormat0 ReadSubTableFormat0(EndianReader reader, int remainingBytes)
        {
            ushort npairs = reader.ReadUInt16();
            ushort searchRange = reader.ReadUInt16();
            ushort entrySelector = reader.ReadUInt16();
            ushort rangeShift = reader.ReadUInt16();
            //----------------------------------------------  
            var ksubTable = new KernSubtableFormat0(npairs);
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
        private KernSubtableFormat2 ReadSubTableFormat2(EndianReader reader, int remainingBytes)
        {
            var beginAt = reader.Position;
            var res = new KernSubtableFormat2();
            res.RowWidth = reader.ReadUInt16();
            var leftClassOffset = reader.ReadUInt16();
            var rightClassOffset = reader.ReadUInt16();
            var kerningArrayOffset = reader.ReadUInt16();
            res.LeftOffset = ReadSubtableClassPair(reader, beginAt + leftClassOffset);
            res.RightOffset = ReadSubtableClassPair(reader, beginAt + rightClassOffset);
            reader.Position = beginAt + kerningArrayOffset;
            // reader.Position = beginAt + leftOffset + rightOffset 
            // reader.ReadInt16()
            return res;
        }

        private KernSubtableClassPair ReadSubtableClassPair(EndianReader reader, long beginAt)
        {
            var firstGlyph = reader.ReadUInt16();
            var nGlyphs = reader.ReadUInt16();
            return new KernSubtableClassPair()
            {
                FirstGlyph = firstGlyph,
                Glyphs = reader.ReadUInt16Array(nGlyphs),
            };
        }

        public override void Write(EndianWriter writer, KernTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
