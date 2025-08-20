using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class VerticalHeaderTable : ITypefaceTable
    {
        public const string TableName = "vhea";

        public string Name => TableName;

        public short VertTypoAscender { get; set; }
        public short VertTypoDescender { get; set; }
        public short VertTypoLineGap { get; set; }
        
        public short AdvanceHeightMax { get; set; }
        public short MinTopSideBearing { get; set; }
        public short MinBottomSideBearing { get; set; }
        
        public short YMaxExtend { get; set; }
        public short CaretSlopeRise { get; set; }
        public short CaretSlopeRun { get; set; }
        public short CaretOffset { get; set; }
        public ushort NumOfLongVerMetrics { get; set; }
    }
}
