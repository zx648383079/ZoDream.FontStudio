using SkiaSharp;
using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Font
{
    public class GlyphContour : ICloneable
    {
        public SKPoint Offset { get; set; }
        public SKPoint Position { get; set; }

        public IList<IGlyphSegment> Segments { get; set; } = [];

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
