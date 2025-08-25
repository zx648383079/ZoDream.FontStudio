using SkiaSharp;
using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Font
{
    public class Glyph : ICloneable
    {
        public float AdvanceWidth { get; set; }

        public SKRect BoundingBox { get; set; }

        public SKPoint SideBearings { get; set; }

        public IList<GlyphContour> Contours { get; set; } = [];

        public float Width => BoundingBox.Width;

        public float Height => BoundingBox.Height;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
