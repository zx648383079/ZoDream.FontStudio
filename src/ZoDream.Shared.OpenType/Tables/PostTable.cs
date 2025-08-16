using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class PostTable : ITypefaceTable
    {
        public const string TableName = "post";

        public string Name => TableName;
    }
}
