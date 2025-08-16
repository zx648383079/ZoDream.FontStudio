using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class EmbeddedBitmapDataTable : ITypefaceTable
    {
        public const string TableName = "EBDT";

        public string Name => TableName;
    }
}
