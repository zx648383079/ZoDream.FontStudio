using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class BitmapSizeConverter : TypefaceConverter<BitmapSizeTable>
    {
        public override BitmapSizeTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return Read(reader);
        }

        public static BitmapSizeTable Read(EndianReader reader)
        {
            var res = new BitmapSizeTable();

            res.IndexSubTableArrayOffset = reader.ReadUInt32();
            res.IndexTablesSize = reader.ReadUInt32();
            res.NumberOfIndexSubTables = reader.ReadUInt32();
            res.ColorRef = reader.ReadUInt32();

            res.Hori = ReadSbitLineMetrics(reader);
            res.Vert = ReadSbitLineMetrics(reader);


            res.StartGlyphIndex = reader.ReadUInt16();
            res.EndGlyphIndex = reader.ReadUInt16();
            res.PpemX = reader.ReadByte();
            res.PpemY = reader.ReadByte();
            res.BitDepth = reader.ReadByte();
            res.Flags = (sbyte)reader.ReadByte();

            return res;
        }

        private static SbitLineMetrics ReadSbitLineMetrics(EndianReader reader)
        {
            return new SbitLineMetrics()
            {
                ascender = (sbyte)reader.ReadByte(),
                descender = (sbyte)reader.ReadByte(),
                widthMax = reader.ReadByte(),
                caretSlopeNumerator = (sbyte)reader.ReadByte(),
                caretSlopeDenominator = (sbyte)reader.ReadByte(),
                caretOffset = (sbyte)reader.ReadByte(),
                minOriginSB = (sbyte)reader.ReadByte(),
                minAdvanceSB = (sbyte)reader.ReadByte(),
                maxBeforeBL = (sbyte)reader.ReadByte(),
                minAfterBL = (sbyte)reader.ReadByte(),
                pad1 = (sbyte)reader.ReadByte(),
                pad2 = (sbyte)reader.ReadByte()
            };
        }

        public override void Write(EndianWriter writer, BitmapSizeTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
