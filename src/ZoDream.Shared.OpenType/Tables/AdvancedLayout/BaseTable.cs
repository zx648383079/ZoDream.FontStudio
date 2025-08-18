using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class BaseTable : ITypefaceTable
    {
        public const string TableName = "BASE";

        public string Name => TableName;

        public AxisTable HorizontalAxis { get; internal set; }
        public AxisTable VerticalAxis { get; internal set; }
    }
}
