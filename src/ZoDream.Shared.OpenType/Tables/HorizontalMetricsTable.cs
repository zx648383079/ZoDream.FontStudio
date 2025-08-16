using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalMetricsTable : ITypefaceTable
    {
        public const string TableName = "hmtx";

        public string Name => TableName;
    }
}
