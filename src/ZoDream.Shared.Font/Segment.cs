using SkiaSharp;
using System;
using System.Numerics;

namespace ZoDream.Shared.Font
{
    public interface IGlyphSegment : ICloneable
    {

    }

    public class LinearSegment(SKPoint point) : IGlyphSegment
    {
        public SKPoint Point { get; set; } = point;

        

        public static explicit operator LinearSegment(SKPoint point)
        {
            return new LinearSegment(point);
        }

        public object Clone()
        {
            return new LinearSegment(Point);
        }
    }

    public class QuadraticBezierSegment(SKPoint controlPoint, SKPoint toPoint) : IGlyphSegment
    {
        public SKPoint ControlPoint { get; set; } = controlPoint;
        public SKPoint ToPoint { get; set; } = toPoint;

        public object Clone()
        {
            return new QuadraticBezierSegment(ControlPoint, ToPoint);
        }

        public static explicit operator QuadraticBezierSegment(Vector4 rect)
        {
            return new QuadraticBezierSegment(
                new SKPoint(rect.X, rect.Y), 
                new SKPoint(rect.Z, rect.W));
        }
    }
    public class CubicBezierSegment(SKPoint controlPoint1, SKPoint controlPoint2, SKPoint toPoint) : IGlyphSegment
    {
        public SKPoint ControlPoint1 { get; set; } = controlPoint1;
        public SKPoint ControlPoint2 { get; set; } = controlPoint2;
        public SKPoint ToPoint { get; set; } = toPoint;

        public object Clone()
        {
            return new CubicBezierSegment(ControlPoint1, ControlPoint2, ToPoint);
        }

    }
}
