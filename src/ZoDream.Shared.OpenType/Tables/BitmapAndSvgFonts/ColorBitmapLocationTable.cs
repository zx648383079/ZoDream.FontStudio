using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ColorBitmapLocationTable : ITypefaceTable
    {
        public const string TableName = "CBLC";

        public string Name => TableName;

        public BitmapSizeTable[] BmpSizeTables { get; internal set; }
    }
}
