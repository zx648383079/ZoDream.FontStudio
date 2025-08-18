using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphDataTable : ITypefaceTable
    {
        public const string TableName = "glyf";

        public string Name => TableName;

        public GlyphData[] Glyphs { get; internal set; }
    }
}
