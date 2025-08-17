using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalHeaderTable : ITypefaceTable
    {
        public const string TableName = "hhea";

        public string Name => TableName;

        public uint Version { get; set; }
        public short Ascent { get; set; }
        public short Descent { get; set; }
        public short LineGap { get; set; }
        public ushort AdvancedWidthMax { get; set; }
        public short MinLeftSideBearing { get; set; }
        public short MinRightSideBearing { get; set; }
        public short MaxXExtent { get; set; }
        public short CaretSlopRise { get; set; }
        public short CaretSlopRun { get; set; }
        public short CaretOffset { get; set; }
        public short MetricDataFormat { get; set; }
        public ushort NumberOfHMetrics { get; set; }
    }
}
