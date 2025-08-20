using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalDeviceMetricsTable : ITypefaceTable
    {
        public const string TableName = "hdmx";

        public string Name => TableName;

        public LongHorMetric[] Metrics { get; internal set; }
        public short[] Bearings { get; internal set; }
    }

    public readonly struct LongHorMetric(ushort advanceWidth, short sideBearing)
    {
        public ushort AdvanceWidth { get; }
        public short SideBearing { get; }
    }
}
