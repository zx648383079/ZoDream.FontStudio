using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphDefinitionTable : ITypefaceTable
    {
        public const string TableName = "GDEF";

        public string Name => TableName;
    }
}
