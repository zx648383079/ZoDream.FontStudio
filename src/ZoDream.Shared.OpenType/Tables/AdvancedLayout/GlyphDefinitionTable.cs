using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphDefinitionTable : TableEntry
    {
        public const string TableName = "GDEF";

        public override string Name => TableName;
    }
}
