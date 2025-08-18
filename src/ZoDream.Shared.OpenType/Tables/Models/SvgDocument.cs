using System.IO;

namespace ZoDream.Shared.OpenType.Tables
{
    public class SvgDocument
    {
        public ushort StartGlyphID;
        public ushort EndGlyphID;
        public uint SvgDocOffset;
        public uint SvgDocLength;

        public Stream Buffer;
        public bool Compressed;
    }
}
