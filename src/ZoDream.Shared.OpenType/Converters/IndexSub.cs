using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class IndexSubConverter : TypefaceConverter<IndexSubTableBase>
    {
        public override IndexSubTableBase? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public static IndexSubTableBase? Read(EndianReader reader, BitmapSizeTable bmpSizeTable)
        {
            IndexSubHeader header = new IndexSubHeader(
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadUInt32()
                );

            switch (header.indexFormat)
            {
                case 1:

                    //IndexSubTable1: variable - metrics glyphs with 4 - byte offsets
                    //Type                  Name            Description
                    //IndexSubHeader        header          Header info.
                    //Offset32              offsetArray[]   offsetArray[glyphIndex] + imageDataOffset = glyphData sizeOfArray = (lastGlyph - firstGlyph + 1) + 1 + 1 pad if needed
                    {
                        int nElem = (bmpSizeTable.EndGlyphIndex - bmpSizeTable.StartGlyphIndex + 1);
                        uint[] offsetArray = reader.ReadArray(nElem, reader.ReadUInt32);
                        //check 16 bit align padd
                        var subTable = new IndexSubTable1();
                        subTable.header = header;
                        subTable.offsetArray = offsetArray;
                        return subTable;
                    }
                case 2:
                    //IndexSubTable2: all glyphs have identical metrics
                    //Type                 Name Description
                    //IndexSubHeader       header  Header info.
                    //uint32               imageSize   All the glyphs are of the same size.
                    //BigGlyphMetrics      bigMetrics  All glyphs have the same metrics; glyph data may be compressed, byte-aligned, or bit-aligned.
                    {
                        IndexSubTable2 subtable = new IndexSubTable2();
                        subtable.header = header;
                        subtable.imageSize = reader.ReadUInt32();
                        subtable.BigGlyphMetrics = ReadBigGlyphMetric(reader);
                        return subtable;
                    }

                case 3:
                    //IndexSubTable3: variable - metrics glyphs with 2 - byte offsets
                    //Type                 Name         Description
                    //IndexSubHeader       header       Header info.
                    //Offset16             offsetArray[]   offsetArray[glyphIndex] + imageDataOffset = glyphData sizeOfArray = (lastGlyph - firstGlyph + 1) + 1 + 1 pad if needed
                    {
                        int nElem = (bmpSizeTable.EndGlyphIndex - bmpSizeTable.StartGlyphIndex + 1);
                        ushort[] offsetArray = reader.ReadArray(nElem, reader.ReadUInt16);
                        //check 16 bit align padd
                        var subTable = new IndexSubTable3();
                        subTable.header = header;
                        subTable.offsetArray = offsetArray;
                        return subTable;
                    }
                case 4:
                    //IndexSubTable4: variable - metrics glyphs with sparse glyph codes
                    //Type                Name      Description
                    //IndexSubHeader      header    Header info.
                    //uint32              numGlyphs Array length.
                    //GlyphIdOffsetPair   glyphArray[numGlyphs + 1]   One per glyph.
                    {
                        IndexSubTable4 subTable = new IndexSubTable4();
                        subTable.header = header;

                        uint numGlyphs = reader.ReadUInt32();
                        GlyphIdOffsetPair[] glyphArray = subTable.glyphArray = new GlyphIdOffsetPair[numGlyphs + 1];
                        for (int i = 0; i <= numGlyphs; ++i) //***
                        {
                            glyphArray[i] = new GlyphIdOffsetPair(reader.ReadUInt16(), reader.ReadUInt16());
                        }
                        return subTable;
                    }
                case 5:
                    //IndexSubTable5: constant - metrics glyphs with sparse glyph codes
                    //Type                Name     Description
                    //IndexSubHeader      header  Header info.
                    //uint32              imageSize   All glyphs have the same data size.
                    //BigGlyphMetrics     bigMetrics  All glyphs have the same metrics.
                    //uint32              numGlyphs   Array length.
                    //uint16              glyphIdArray[numGlyphs]     One per glyph, sorted by glyph ID.
                    {
                        IndexSubTable5 subTable = new IndexSubTable5();
                        subTable.header = header;

                        subTable.imageSize = reader.ReadUInt32();
                        subTable.BigGlyphMetrics = ReadBigGlyphMetric(reader);
                        subTable.glyphIdArray = reader.ReadArray((int)reader.ReadUInt32(), reader.ReadUInt16);
                        return subTable;
                    }

            }
            return null;
        }

        private static BigGlyphMetrics ReadBigGlyphMetric(EndianReader reader)
        {
            var res = new BigGlyphMetrics();
            res.height = reader.ReadByte();
            res.width = reader.ReadByte();

            res.horiBearingX = (sbyte)reader.ReadByte();
            res.horiBearingY = (sbyte)reader.ReadByte();
            res.horiAdvance = reader.ReadByte();

            res.vertBearingX = (sbyte)reader.ReadByte();
            res.vertBearingY = (sbyte)reader.ReadByte();
            res.vertAdvance = reader.ReadByte();
            return res;
        }


        public override void Write(EndianWriter writer, IndexSubTableBase data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
