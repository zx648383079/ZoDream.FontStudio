using SkiaSharp;
using System.Linq;
using System.Numerics;

namespace ZoDream.Shared.Font
{
    public class GlyphBuilder
    {
        public GlyphContour Contour { get; set; } = new();

        public Glyph Glyph { get; set; } = new();

        public SKPoint Offset { get; set; } = SKPoint.Empty;

        public SKRotationScaleMatrix Scale { get; set; } = new(1, 0, 0, 1);


        public SKRect BoundingBox 
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

        public SKPoint Transform(SKPoint value)
        {
            return new SKPoint(Scale.SCos * value.X + Scale.SSin * value.Y,
                Scale.TX * value.X + Scale.TY * value.Y);
        }

        public void MoveAbsolute(SKPoint offset)
        {
            var last = Glyph.Contours.Count > 0 ? Glyph.Contours.Last().Position : SKPoint.Empty;
            var arg = Offset + Transform(offset);
            Contour.Offset = arg - last;
            Contour.Position = arg;
        }

        public void MoveRelative(SKPoint offset)
        {
            var arg = Transform(offset);
            Contour.Offset += arg;
            Contour.Position += arg;
        }
        public void MoveControl(SKPoint offset)
        {
            var arg = Transform(offset);
            if (Contour.Segments.Count == 0)
            {
                return;
            }
            switch (Contour.Segments[0])
            {
                case QuadraticBezierSegment q:
                    q.ControlPoint = arg;
                    break;
                case CubicBezierSegment c:
                    c.ControlPoint1 = arg;
                    break;
                default:
                    break;
            }
        }

        public void AddLinear(SKPoint point)
        {
            Contour.Segments.Add(new LinearSegment(Transform(point)));
        }

        public void AddQuadratic(SKPoint point1, SKPoint point2)
        {
            Contour.Segments.Add(new QuadraticBezierSegment(
                Transform(point1), 
                Transform(point2)));
        }

        public void AddCubic(SKPoint point1, SKPoint point2, SKPoint point3)
        {
            Contour.Segments.Add(new CubicBezierSegment(
                Transform(point1), 
                Transform(point2), 
                Transform(point3)));
        }

        public static explicit operator Glyph(GlyphBuilder builder)
        {
            var glyph = (Glyph)builder.Glyph.Clone();
            glyph.SideBearings = new SKPoint(glyph.SideBearings.X,
                glyph.AdvanceWidth - (glyph.SideBearings.X + glyph.Width));
            return glyph;
        }
    }
}
