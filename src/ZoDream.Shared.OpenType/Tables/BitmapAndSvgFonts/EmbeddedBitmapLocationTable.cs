using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class EmbeddedBitmapLocationTable : ITypefaceTable
    {
        internal const int MAX_BITMAP_STRIKES = 1024;
        public const string TableName = "EBLC";

        public string Name => TableName;

        public BitmapSizeTable[] BmpSizeTables { get; internal set; }
    }
}
