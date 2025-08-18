using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class VerticalMetricsTable : ITypefaceTable
    {
        public const string TableName = "vmtx";

        public string Name => TableName;

        public AdvanceHeightAndTopSideBearing[] AdvHeightAndTopSideBearings {  get; set; }
    }

}
