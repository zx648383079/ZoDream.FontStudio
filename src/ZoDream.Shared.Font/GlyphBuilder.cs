using System.Linq;
using System.Numerics;

namespace ZoDream.Shared.Font
{
    public class GlyphBuilder
    {
        public GlyphContour Contour { get; set; } = new();

        public Glyph Glyph { get; set; } = new();

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public Vector4 Scale { get; set; } = new(1, 0, 0, 1);


        public Vector4 BoundingBox 
        {
            get => Glyph.BoundingBox; 
            set => Glyph.BoundingBox = value;
        }

        public Vector2 HorizontalMetrics {
            get => new(Glyph.AdvanceWidth, Glyph.SideBearings.X);
            set {
                Glyph.AdvanceWidth = value.X;
                Glyph.SideBearings = new(value.Y, Glyph.SideBearings.Y);
            }
        }


        public void Flush()
        {
            if (Contour.Segments.Count == 0)
            {
                return;
            }
            Glyph.Contours.Add(Contour);
            Contour = new();
        }

        public Vector2 Transform(Vector2 value)
        {
            return new Vector2(Scale.X * value.X + Scale.Y * value.Y,
                Scale.Z * value.X + Scale.W * value.Y);
        }

        public void MoveAbsolute(Vector2 offset)
        {
            var last = Glyph.Contours.Count > 0 ? Glyph.Contours.Last().Position : Vector2.Zero;
            var arg = Offset + Transform(offset);
            Contour.Offset = arg - last;
            Contour.Position = arg;
        }

        public void MoveRelative(Vector2 offset)
        {
            var arg = Transform(offset);
            Contour.Offset += arg;
            Contour.Position += arg;
        }
        public void MoveControl(Vector2 offset)
        {
            var arg = Transform(offset);
            if (Contour.Segments.Count == 0)
            {
                return;
            }
            switch (Contour.Segments[0])
            {
                case QuadraticBezierSegment q:
                    q.Point1 = arg;
                    break;
                case CubicBezierSegment c:
                    c.Point1 = arg;
                    break;
                default:
                    break;
            }
        }

        public void AddLinear(Vector2 point)
        {
            Contour.Segments.Add(new LinearSegment(Transform(point)));
        }

        public void AddQuadratic(Vector2 point1, Vector2 point2)
        {
            Contour.Segments.Add(new QuadraticBezierSegment(
                Transform(point1), 
                Transform(point2)));
        }

        public void AddCubic(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            Contour.Segments.Add(new CubicBezierSegment(
                Transform(point1), 
                Transform(point2), 
                Transform(point3)));
        }

        public static explicit operator Glyph(GlyphBuilder builder)
        {
            var glyph = (Glyph)builder.Glyph.Clone();
            glyph.SideBearings = new Vector2(glyph.SideBearings.X,
                glyph.AdvanceWidth - (glyph.SideBearings.X + glyph.Width));
            return glyph;
        }
    }
}
