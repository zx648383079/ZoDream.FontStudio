using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class EmbeddedBitmapScalingTable : ITypefaceTable
    {
        public const string TableName = "EBSC";

        public string Name => TableName;

        public BitmapScale[] Strikes { get; internal set; }
    }

    public class BitmapScale
    {
        public SbitLineMetrics Hori { get; internal set; }
        public SbitLineMetrics Vert { get; internal set; }
        public byte PpemX { get; internal set; }
        public byte PpemY { get; internal set; }
        public byte SubstitutePpemX { get; internal set; }
        public byte SubstitutePpemY { get; internal set; }
    }
}
