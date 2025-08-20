using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class EmbeddedBitmapLocationConverter : TypefaceConverter<EmbeddedBitmapLocationTable>
    {
        public override EmbeddedBitmapLocationTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new EmbeddedBitmapLocationTable();
            long beginAt = reader.BaseStream.Position;

            ushort versionMajor = reader.ReadUInt16();
            ushort versionMinor = reader.ReadUInt16();
            uint numSizes = reader.ReadUInt32();

            if (numSizes > EmbeddedBitmapLocationTable.MAX_BITMAP_STRIKES)
            {
                throw new Exception("Too many bitmap strikes in font.");
            }

            //----------------
            var bmpSizeTables = new BitmapSizeTable[numSizes];
            for (int i = 0; i < numSizes; i++)
            {
                bmpSizeTables[i] = BitmapSizeConverter.Read(reader);
            }
            res.BmpSizeTables = bmpSizeTables;

            for (int n = 0; n < numSizes; ++n)
            {
                var bmpSizeTable = bmpSizeTables[n];
                uint numberofIndexSubTables = bmpSizeTable.NumberOfIndexSubTables;

                var indexSubTableArrs = new IndexSubTableArray[numberofIndexSubTables];
                for (uint i = 0; i < numberofIndexSubTables; ++i)
                {
                    indexSubTableArrs[i] = new IndexSubTableArray(
                             reader.ReadUInt16(), //First glyph ID of this range.
                             reader.ReadUInt16(), //Last glyph ID of this range (inclusive).
                             reader.ReadUInt32());//Add to indexSubTableArrayOffset to get offset from beginning of EBLC.                      
                }

                var subTables = new IndexSubTableBase[numberofIndexSubTables];
                bmpSizeTable.IndexSubTables = subTables;
                for (uint i = 0; i < numberofIndexSubTables; ++i)
                {
                    var indexSubTableArr = indexSubTableArrs[i];
                    reader.BaseStream.Position = beginAt + bmpSizeTable.IndexSubTableArrayOffset + indexSubTableArr.AdditionalOffsetToIndexSubtable;

                    subTables[i] = IndexSubConverter.Read(reader, bmpSizeTable);
                }
            }
            return res;
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapLocationTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
