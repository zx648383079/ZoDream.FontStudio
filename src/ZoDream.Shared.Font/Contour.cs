using System;
using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.Font
{
    public class GlyphContour : ICloneable
    {
        public Vector2 Offset { get; set; }
        public Vector2 Position { get; set; }

        public IList<IGlyphSegment> Segments { get; set; } = [];

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
