using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalDeviceMetricsTable : ITypefaceTable
    {
        public const string TableName = "hdmx";

        public string Name => TableName;
    }
}
