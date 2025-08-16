using System;
using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.Font
{
    public class Glyph : ICloneable
    {
        public float AdvanceWidth { get; set; }

        public Vector4 BoundingBox { get; set; }

        public Vector2 SideBearings { get; set; }

        public IList<GlyphContour> Contours { get; set; } = [];

        public float Width => BoundingBox.Z - BoundingBox.X;

        public float Height => BoundingBox.W - BoundingBox.Y;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
