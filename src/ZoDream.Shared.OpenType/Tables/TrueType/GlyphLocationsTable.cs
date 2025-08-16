using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphLocationsTable : ITypefaceTable
    {
        public const string TableName = "loca";

        public string Name => TableName;
    }
}
