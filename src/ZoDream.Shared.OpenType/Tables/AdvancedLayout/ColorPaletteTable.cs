using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ColorPaletteTable : ITypefaceTable
    {
        public const string TableName = "CPAL";

        public string Name => TableName;

        public ushort[] Palettes { get; set; }
        public Color[] Colors { get; set; }
    }
}
