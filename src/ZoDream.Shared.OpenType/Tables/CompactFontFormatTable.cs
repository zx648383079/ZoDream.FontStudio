using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class CompactFontFormatTable : TableEntry
    {
        public const string TableName = "CFF ";

        public override string Name => TableName;
    }

    public class CompactFontFormat2Table : TableEntry
    {
        public const string TableName = "CFF2";

        public override string Name => TableName;
    }
}
