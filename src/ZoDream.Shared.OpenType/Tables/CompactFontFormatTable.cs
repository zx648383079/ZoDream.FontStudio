using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class CompactFontFormatTable : ITypefaceTable
    {
        public const string TableName = "CFF ";

        public string Name => TableName;
    }

    public class CompactFontFormat2Table : ITypefaceTable
    {
        public const string TableName = "CFF2";

        public string Name => TableName;
    }
}
