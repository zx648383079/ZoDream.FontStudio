using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable4 : IndexSubTableBase
    {
        public override int SubTypeNo => 4;
        public GlyphIdOffsetPair[] GlyphArray;

        public override void BuildGlyphList(List<GlyphData> glyphList)
        {
            for (int i = 0; i < GlyphArray.Length; ++i)
            {
                var pair = GlyphArray[i];
                glyphList.Add(new(pair.GlyphId, Header.ImageDataOffset + pair.Offset, 0, Header.ImageFormat));
            }
        }
    }
}
