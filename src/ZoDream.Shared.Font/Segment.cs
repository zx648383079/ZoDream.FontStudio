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

    public class QuadraticBezierSegment(SKPoint point1, SKPoint point2) : IGlyphSegment
    {
        public SKPoint Point1 { get; set; } = point1;
        public SKPoint Point2 { get; set; } = point2;

        public object Clone()
        {
            return new QuadraticBezierSegment(Point1, Point2);
        }

        public static explicit operator QuadraticBezierSegment(Vector4 rect)
        {
            return new QuadraticBezierSegment(
                new SKPoint(rect.X, rect.Y), 
                new SKPoint(rect.Z, rect.W));
        }
    }
    public class CubicBezierSegment(SKPoint point1, SKPoint point2, SKPoint point3) : IGlyphSegment
    {
        public SKPoint Point1 { get; set; } = point1;
        public SKPoint Point2 { get; set; } = point2;
        public SKPoint Point3 { get; set; } = point3;

        public object Clone()
        {
            return new CubicBezierSegment(Point1, Point2, Point3);
        }

    }
}
