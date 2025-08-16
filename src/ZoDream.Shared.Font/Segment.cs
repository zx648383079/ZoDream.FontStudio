using System;
using System.Numerics;

namespace ZoDream.Shared.Font
{
    public interface IGlyphSegment : ICloneable
    {

    }

    public class LinearSegment(Vector2 point) : IGlyphSegment
    {
        public Vector2 Point { get; set; } = point;

        

        public static explicit operator LinearSegment(Vector2 point)
        {
            return new LinearSegment(point);
        }

        public object Clone()
        {
            return new LinearSegment(Point);
        }
    }

    public class QuadraticBezierSegment(Vector2 point1, Vector2 point2) : IGlyphSegment
    {
        public Vector2 Point1 { get; set; } = point1;
        public Vector2 Point2 { get; set; } = point2;

        public object Clone()
        {
            return new QuadraticBezierSegment(Point1, Point2);
        }

        public static explicit operator QuadraticBezierSegment(Vector4 rect)
        {
            return new QuadraticBezierSegment(
                new Vector2(rect.X, rect.Y), 
                new Vector2(rect.Z, rect.W));
        }
    }
    public class CubicBezierSegment(Vector2 point1, Vector2 point2, Vector2 point3) : IGlyphSegment
    {
        public Vector2 Point1 { get; set; } = point1;
        public Vector2 Point2 { get; set; } = point2;
        public Vector2 Point3 { get; set; } = point3;

        public object Clone()
        {
            return new CubicBezierSegment(Point1, Point2, Point3);
        }

    }
}
