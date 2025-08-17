using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class VerticalDeviceMetricsTable : ITypefaceTable
    {
        public const string TableName = "VDMX";

        public string Name => TableName;

        public Ratio[] Ratios { get; set; }
    }

    public struct Ratio(byte charset, byte xRatio, byte yStartRatio, byte yEndRatio)
    {
        public readonly byte Charset => charset;
        public readonly byte XRatio => xRatio;
        public readonly byte YStartRatio => yStartRatio;
        public readonly byte YEndRatio => yEndRatio;
    }
}
