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
            var header = new IndexSubHeader(
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadUInt32()
                );

            switch (header.IndexFormat)
            {
                case 1:
                    {
                        int nElem = (bmpSizeTable.EndGlyphIndex - bmpSizeTable.StartGlyphIndex + 1);
                        uint[] offsetArray = reader.ReadArray(nElem, reader.ReadUInt32);
                        //check 16 bit align padd
                        var subTable = new IndexSubTable1
                        {
                            Header = header,
                            OffsetArray = offsetArray
                        };
                        return subTable;
                    }
                case 2:
                    {
                        var subtable = new IndexSubTable2
                        {
                            Header = header,
                            ImageSize = reader.ReadUInt32(),
                            BigGlyphMetrics = MetricsConverter.ReadBig(reader)
                        };
                        return subtable;
                    }

                case 3:
                    {
                        int nElem = (bmpSizeTable.EndGlyphIndex - bmpSizeTable.StartGlyphIndex + 1);
                        var offsetArray = reader.ReadUInt16Array(nElem);
                        var subTable = new IndexSubTable3
                        {
                            Header = header,
                            offsetArray = offsetArray
                        };
                        return subTable;
                    }
                case 4:
                    {
                        var subTable = new IndexSubTable4
                        {
                            Header = header
                        };

                        uint numGlyphs = reader.ReadUInt32();
                        var glyphArray = subTable.GlyphArray = new GlyphIdOffsetPair[numGlyphs + 1];
                        for (int i = 0; i <= numGlyphs; ++i) //***
                        {
                            glyphArray[i] = new GlyphIdOffsetPair(reader.ReadUInt16(), reader.ReadUInt16());
                        }
                        return subTable;
                    }
                case 5:
                    {
                        var subTable = new IndexSubTable5
                        {
                            Header = header,

                            ImageSize = reader.ReadUInt32(),
                            BigGlyphMetrics = MetricsConverter.ReadBig(reader),
                            GlyphIdArray = reader.ReadArray((int)reader.ReadUInt32(), reader.ReadUInt16)
                        };
                        return subTable;
                    }

            }
            return null;
        }


        public override void Write(EndianWriter writer, IndexSubTableBase data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
