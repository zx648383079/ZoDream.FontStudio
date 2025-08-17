using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/typography/opentype/spec/cmap
    /// </summary>
    public class CharacterGlyphMappingTable : ITypefaceTable
    {
        public const string TableName = "cmap";

        public string Name => TableName;

        public CharacterMap[] CharacterMaps { get; set; }
    }
}
