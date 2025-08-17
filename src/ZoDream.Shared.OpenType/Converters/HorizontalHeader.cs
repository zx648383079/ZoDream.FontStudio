using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class HorizontalHeaderConverter : TypefaceConverter<HorizontalHeaderTable>
    {
        public override HorizontalHeaderTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new HorizontalHeaderTable();
            res.Version = reader.ReadUInt32(); //major + minor
            res.Ascent = reader.ReadInt16();
            res.Descent = reader.ReadInt16();
            res.LineGap = reader.ReadInt16();

            res.AdvancedWidthMax = reader.ReadUInt16();
            res.MinLeftSideBearing = reader.ReadInt16();
            res.MinRightSideBearing = reader.ReadInt16();
            res.MaxXExtent = reader.ReadInt16();

            res.CaretSlopRise = reader.ReadInt16();
            res.CaretSlopRun = reader.ReadInt16();
            res.CaretOffset = reader.ReadInt16();

            //reserve 4 int16 fields, int16 x 4 fields
            reader.BaseStream.Seek(2 * 4, SeekOrigin.Current);

            res.MetricDataFormat = reader.ReadInt16(); // 0
            res.NumberOfHMetrics = reader.ReadUInt16();
            return res;
        }

        public override void Write(EndianWriter writer, HorizontalHeaderTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
