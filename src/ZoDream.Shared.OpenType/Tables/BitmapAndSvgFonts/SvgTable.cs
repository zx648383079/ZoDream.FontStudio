using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class SvgTable : ITypefaceTable
    {
        public const string TableName = "SVG ";

        public string Name => TableName;
    }
}
