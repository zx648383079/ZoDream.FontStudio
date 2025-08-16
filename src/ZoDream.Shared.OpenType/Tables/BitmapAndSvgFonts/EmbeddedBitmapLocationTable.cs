using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class EmbeddedBitmapLocationTable : ITypefaceTable
    {
        public const string TableName = "EBLC";

        public string Name => TableName;
    }
}
