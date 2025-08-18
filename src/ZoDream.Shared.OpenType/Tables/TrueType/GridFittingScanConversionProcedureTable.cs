using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GridFittingScanConversionProcedureTable : ITypefaceTable
    {
        public const string TableName = "gasp";

        public string Name => TableName;

        public GaspRangeRecord[] RangeRecords {  get; set; }
    }
}
