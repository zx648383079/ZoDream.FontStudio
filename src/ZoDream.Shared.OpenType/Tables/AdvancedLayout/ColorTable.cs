using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ColorTable : ITypefaceTable
    {
        public const string TableName = "COLR";

        public string Name => TableName;
    }
}
