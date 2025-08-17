using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class VerticalHeaderConverter : TypefaceConverter<VerticalHeaderTable>
    {
        public override VerticalHeaderTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new VerticalHeaderTable();
            var version = reader.ReadUInt32();
            res.VersionMajor = (byte)(version >> 16);
            res.VersionMinor = (byte)(version >> 8);

            res.VertTypoAscender = reader.ReadInt16();
            res.VertTypoDescender = reader.ReadInt16();
            res.VertTypoLineGap = reader.ReadInt16();
            
            res.AdvanceHeightMax = reader.ReadInt16();
            res.MinTopSideBearing = reader.ReadInt16();
            res.MinBottomSideBearing = reader.ReadInt16();
            
            res.YMaxExtend = reader.ReadInt16();
            res.CaretSlopeRise = reader.ReadInt16();
            res.CaretSlopeRun = reader.ReadInt16();
            res.CaretOffset = reader.ReadInt16();
            
            //skip 5 int16 =>  4 reserve field + 1 metricDataFormat            
            reader.BaseStream.Position += (2 * (4 + 1)); //short = 2 byte, 
            
            res.NumOfLongVerMetrics = reader.ReadUInt16();

            return res;
        }

        public override void Write(EndianWriter writer, VerticalHeaderTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
