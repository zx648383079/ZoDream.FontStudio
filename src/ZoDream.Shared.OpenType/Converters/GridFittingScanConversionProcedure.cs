using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GridFittingScanConversionProcedureConverter : TypefaceConverter<GridFittingScanConversionProcedureTable>
    {
        public override GridFittingScanConversionProcedureTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new GridFittingScanConversionProcedureTable();
            var version = reader.ReadUInt16();
            var numRanges = reader.ReadUInt16();
            res.RangeRecords = reader.ReadArray(numRanges, () => {
                return new GaspRangeRecord(reader.ReadUInt16(),
                    (GaspRangeBehavior)reader.ReadUInt16());
            });
            return res;
        }

        public override void Write(EndianWriter writer, GridFittingScanConversionProcedureTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
