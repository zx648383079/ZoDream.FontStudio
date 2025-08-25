using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Font
{
    public class FontBuilder
    {
        public FontMetrics Metrics { get; set; } = new();

        public FontMeta Meta { get; set; } = new();

        public Dictionary<ushort, string> CharacterMapping { get; set; } = [];

        public IList<GlyphBuilder> Glyphs { get; set; } = [];

        public Dictionary<Tuple<string, string>, ushort> KerningPairs = [];
    }
}
