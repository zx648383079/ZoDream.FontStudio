using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class FontGlyphData
    {
        public string Name { get; internal set; }
        public ushort SIDName { get; internal set; }
        internal Type2Instruction[] GlyphInstructions { get; set; }
    }
}
