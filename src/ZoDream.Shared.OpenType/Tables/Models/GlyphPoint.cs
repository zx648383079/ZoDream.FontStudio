using System.Numerics;

namespace ZoDream.Shared.OpenType.Tables
{
    public struct GlyphPoint
    {
        internal Vector2 P;
        internal bool OnCurve;

        public GlyphPoint(float x, float y, bool onCurve)
        {
            P = new Vector2(x, y);
            OnCurve = onCurve;
        }
        public GlyphPoint(Vector2 position, bool onCurve)
        {
            P = position;
            OnCurve = onCurve;
        }
        public float X => P.X;
        public float Y => P.Y;

        public static GlyphPoint operator *(GlyphPoint p, float n)
        {
            return new GlyphPoint(p.P * n, p.OnCurve);
        }

        //-----------------------------------------

        internal GlyphPoint Offset(short dx, short dy) 
        { 
            return new GlyphPoint(new Vector2(P.X + dx, P.Y + dy), OnCurve); 
        }

        internal void ApplyScale(float scale)
        {
            P *= scale;
        }
        internal void ApplyScaleOnlyOnXAxis(float scale)
        {
            P = new Vector2(P.X * scale, P.Y);
        }

        internal void UpdateX(float x)
        {
            P.X = x;
        }
        internal void UpdateY(float y)
        {
            P.Y = y;
        }
        internal void OffsetY(float dy)
        {
            P.Y += dy;
        }
        internal void OffsetX(float dx)
        {
            P.X += dx;
        }
    }
}
