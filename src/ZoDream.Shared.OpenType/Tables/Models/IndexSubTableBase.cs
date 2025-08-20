using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class IndexSubTableBase
    {
        public IndexSubHeader Header;

        public abstract int SubTypeNo { get; }
        public ushort FirstGlyphIndex;
        public ushort LastGlyphIndex;

        public abstract void BuildGlyphList(List<GlyphData> glyphList);
    }
}
