using System.Collections.Generic;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ColorTable : ITypefaceTable
    {
        public const string TableName = "COLR";

        public string Name => TableName;

        public ushort[] GlyphLayers { get; set; }
        public ushort[] GlyphPalettes { get; set; }
        public readonly Dictionary<ushort, ushort> LayerIndices = [];
        public readonly Dictionary<ushort, ushort> LayerCounts = [];
    }
}
