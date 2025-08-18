using System.Collections.Generic;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class FontFamily
    {
        internal string FontName { get; set; }
        internal GlyphData[] _glyphs;

        internal List<byte[]> _localSubrRawBufferList;
        internal List<byte[]> _globalSubrRawBufferList;

        internal int _defaultWidthX;
        internal int _nominalWidthX;
        internal List<FontDict> _cidFontDict;

        public string Version { get; set; } //CFF SID
        public string Notice { get; set; }//CFF SID
        public string CopyRight { get; set; }//CFF SID
        public string FullName { get; set; }//CFF SID        
        public string FamilyName { get; set; }//CFF SID
        public string Weight { get; set; }//CFF SID 
        public double UnderlinePosition { get; set; }
        public double UnderlineThickness { get; set; }
        public double[] FontBBox { get; set; }
    }
}
