using System.IO;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class SvgTable : ITypefaceTable
    {
        public const string TableName = "SVG ";

        public string Name => TableName;

        public SvgDocument[] Bodies { get; set; }
    }

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
