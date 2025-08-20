using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable2 : IndexSubTableBase
    {
        public override int SubTypeNo => 2;
        public uint ImageSize;
        public BigGlyphMetrics BigGlyphMetrics = new();

        public override void BuildGlyphList(List<GlyphData> glyphList)
        {
            uint incrementalOffset = 0;//TODO: review this
            for (ushort n = FirstGlyphIndex; n <= LastGlyphIndex; ++n)
            {
                glyphList.Add(new(n, Header.ImageDataOffset + incrementalOffset, ImageSize, Header.ImageFormat));
                incrementalOffset += ImageSize;
            }
        }
    }
}
