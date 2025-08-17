using System;

namespace ZoDream.Shared.OpenType
{
    public readonly struct GlyphBound(short xMin, short yMin, short xMax, short yMax): IEquatable<GlyphBound>
    {

        public static readonly GlyphBound Zero = new(0, 0, 0, 0);

        public short XMin { get; } = xMin;
        public short YMin { get; } = yMin;
        public short XMax { get; } = xMax;
        public short YMax { get; } = yMax;

        public readonly bool Equals(GlyphBound other)
        {
            return XMin == other.XMin && YMin == other.YMin && XMax == other.XMax && YMax == other.YMax;
        }

        public override readonly string ToString()
        {
            return $"{{{XMin},{YMin},{XMax},{YMax}}}";
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(XMin, YMin, XMax, YMax);
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is GlyphBound o)
            {
                return Equals(o);
            }
            return base.Equals(obj);
        }
        public static bool operator ==(GlyphBound left, GlyphBound right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GlyphBound left, GlyphBound right)
        {
            return !(left == right);
        }
    }
}
