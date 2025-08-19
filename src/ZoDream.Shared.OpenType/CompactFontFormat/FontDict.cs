using System.Collections.Generic;
using System.IO;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class FontDict
    {
        public int FontName;
        public int PrivateDicSize;
        public int PrivateDicOffset;
        public List<Stream> LocalSubr;
        public FontDict(int dictSize, int dictOffset)
        {
            PrivateDicSize = dictSize;
            PrivateDicOffset = dictOffset;
        }

    }
}
