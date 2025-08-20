using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable1 : IndexSubTableBase
    {
        public override int SubTypeNo => 1;
        public uint[] OffsetArray;

        public override void BuildGlyphList(List<GlyphData> glyphList)
        {
            int n = 0;
            for (ushort i = FirstGlyphIndex; i <= LastGlyphIndex; ++i)
            {
                glyphList.Add(new(i, Header.ImageDataOffset + OffsetArray[n], 0, Header.ImageFormat));
                n++;
            }
        }
    }
}
