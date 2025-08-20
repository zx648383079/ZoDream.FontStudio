using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class IndexSubTable5 : IndexSubTableBase
    {
        public override int SubTypeNo => 5;
        public uint ImageSize;
        public BigGlyphMetrics BigGlyphMetrics = new();

        public ushort[] GlyphIdArray;

        public override void BuildGlyphList(List<GlyphData> glyphList)
        {
            uint incrementalOffset = 0;//TODO: review this
            for (int i = 0; i < GlyphIdArray.Length; ++i)
            {
                glyphList.Add(new(GlyphIdArray[i], Header.ImageDataOffset + incrementalOffset, ImageSize, Header.ImageFormat));
                incrementalOffset += ImageSize;
            }
        }
    }
}
