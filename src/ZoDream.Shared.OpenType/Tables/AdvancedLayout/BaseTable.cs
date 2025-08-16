using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class BaseTable : ITypefaceTable
    {
        public const string TableName = "BASE";

        public string Name => TableName;
    }
}
