using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class VerticalDeviceMetricsTable : ITypefaceTable
    {
        public const string TableName = "VDMX";

        public string Name => TableName;
    }
}
