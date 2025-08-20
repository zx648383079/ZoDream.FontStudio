using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable3 : IndexSubTableBase
    {
        public override int SubTypeNo => 3;
        public ushort[] offsetArray;

        public override void BuildGlyphList(List<GlyphData> glyphList)
        {
            int n = 0;
            for (ushort i = FirstGlyphIndex; i <= LastGlyphIndex; ++i)
            {
                glyphList.Add(new(i, Header.ImageDataOffset + offsetArray[n++], 0, Header.ImageFormat));
            }
        }
    }
}
