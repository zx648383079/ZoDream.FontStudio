using System.IO;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ColorBitmapDataTable : ITypefaceTable
    {
        public const string TableName = "CBDT";

        public string Name => TableName;

        public Stream Buffer { get; set; }
    }
}
