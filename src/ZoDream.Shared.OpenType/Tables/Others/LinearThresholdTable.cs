using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LinearThresholdTable : ITypefaceTable
    {
        public const string TableName = "LTSH";

        public string Name => TableName;

        public byte[] yPixels { get; internal set; }
    }
}
