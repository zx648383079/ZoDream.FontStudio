using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ColorBitmapLocationConverter : TypefaceConverter<ColorBitmapLocationTable>
    {
        public override ColorBitmapLocationTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new ColorBitmapLocationTable();
            long cblcBeginPos = reader.BaseStream.Position;
            ushort majorVersion = reader.ReadUInt16(); //3
            ushort minorVersion = reader.ReadUInt16(); //0
            uint numSizes = reader.ReadUInt32();
            var bmpSizeTables = new BitmapSizeTable[numSizes];
            for (int i = 0; i < numSizes; ++i)
            {
                bmpSizeTables[i] = BitmapSizeConverter.Read(reader);
            }
            res.BmpSizeTables = bmpSizeTables;
            for (int n = 0; n < numSizes; ++n)
            {
                var bmpSizeTable = bmpSizeTables[n];
                uint numberOfIndexSubTables = bmpSizeTable.NumberOfIndexSubTables;

                var indexSubTableArrs = new IndexSubTableArray[numberOfIndexSubTables];
                for (uint i = 0; i < numberOfIndexSubTables; ++i)
                {
                    indexSubTableArrs[i] = new IndexSubTableArray(
                             reader.ReadUInt16(), //First glyph ID of this range.
                             reader.ReadUInt16(), //Last glyph ID of this range (inclusive).
                             reader.ReadUInt32());//Add to indexSubTableArrayOffset to get offset from beginning of EBLC.                      
                }

                var subTables = new IndexSubTableBase[numberOfIndexSubTables];
                bmpSizeTable.IndexSubTables = subTables;
                for (uint i = 0; i < numberOfIndexSubTables; ++i)
                {
                    IndexSubTableArray indexSubTableArr = indexSubTableArrs[i];
                    reader.BaseStream.Position = cblcBeginPos + bmpSizeTable.IndexSubTableArrayOffset + indexSubTableArr.additionalOffsetToIndexSubtable;

                    IndexSubTableBase result = subTables[i] = IndexSubConverter.Read(reader, bmpSizeTable);
                    result.FirstGlyphIndex = indexSubTableArr.FirstGlyphIndex;
                    result.LastGlyphIndex = indexSubTableArr.LastGlyphIndex;
                }
            }
            return res;
        }


        public override void Write(EndianWriter writer, ColorBitmapLocationTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
