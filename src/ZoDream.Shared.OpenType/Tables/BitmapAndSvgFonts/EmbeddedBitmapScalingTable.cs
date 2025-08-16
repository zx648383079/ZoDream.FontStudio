using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class EmbeddedBitmapScalingTable : ITypefaceTable
    {
        public const string TableName = "EBSC";

        public string Name => TableName;
    }
}
