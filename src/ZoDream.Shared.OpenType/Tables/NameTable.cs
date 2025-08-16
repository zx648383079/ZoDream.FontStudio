using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class NameTable : TableEntry
    {
        public const string TableName = "name";

        public override string Name => TableName;
    }
}
