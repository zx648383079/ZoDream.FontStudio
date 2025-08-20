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
            var res = new BitmapSizeTable
            {
                IndexSubTableArrayOffset = reader.ReadUInt32(),
                IndexTablesSize = reader.ReadUInt32(),
                NumberOfIndexSubTables = reader.ReadUInt32(),
                ColorRef = reader.ReadUInt32(),

                Horizontal = MetricsConverter.ReadSbitLine(reader),
                Vertical = MetricsConverter.ReadSbitLine(reader),


                StartGlyphIndex = reader.ReadUInt16(),
                EndGlyphIndex = reader.ReadUInt16(),
                PpemX = reader.ReadByte(),
                PpemY = reader.ReadByte(),
                BitDepth = reader.ReadByte(),
                Flags = reader.ReadSByte()
            };

            return res;
        }

    

        public override void Write(EndianWriter writer, BitmapSizeTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
