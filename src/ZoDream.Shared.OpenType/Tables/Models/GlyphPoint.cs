using System.Numerics;

namespace ZoDream.Shared.OpenType.Tables
{
    public struct GlyphPoint
    {
        internal Vector2 P;
        internal bool onCurve;

        public GlyphPoint(float x, float y, bool onCurve)
        {
            P = new Vector2(x, y);
            this.onCurve = onCurve;
        }
        public GlyphPoint(Vector2 position, bool onCurve)
        {
            P = position;
            this.onCurve = onCurve;
        }
        public float X => this.P.X;
        public float Y => this.P.Y;

        public static GlyphPoint operator *(GlyphPoint p, float n)
        {
            return new GlyphPoint(p.P * n, p.onCurve);
        }

        //-----------------------------------------

        internal GlyphPoint Offset(short dx, short dy) 
        { 
            return new GlyphPoint(new Vector2(P.X + dx, P.Y + dy), onCurve); 
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
            this.P.X = x;
        }
        internal void UpdateY(float y)
        {
            this.P.Y = y;
        }
        internal void OffsetY(float dy)
        {
            this.P.Y += dy;
        }
        internal void OffsetX(float dx)
        {
            this.P.X += dx;
        }
    }
}
