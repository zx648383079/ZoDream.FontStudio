using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class KernTable : ITypefaceTable
    {
        public const string TableName = "kern";

        public string Name => TableName;
    }
}
