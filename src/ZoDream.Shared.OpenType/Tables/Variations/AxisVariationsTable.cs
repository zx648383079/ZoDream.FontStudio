using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisVariationsTable : ITypefaceTable
    {
        public const string TableName = "avar";

        public string Name => TableName;

        public AxisValuePair[][] AxisSegmentMaps;
    }


}
