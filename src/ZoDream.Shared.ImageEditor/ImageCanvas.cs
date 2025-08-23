using SkiaSharp;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor
{
    public class ImageCanvas(SKCanvas canvas) : IImageCanvas
    {
        public void DrawBitmap(SKBitmap source)
        {
            DrawBitmap(source, Vector2.Zero);
        }

        public void DrawBitmap(SKBitmap source, Vector2 point)
        {
            DrawBitmap(source, new Vector4(point.X, point.Y, source.Width, source.Height));
        }

        public void DrawBitmap(SKBitmap source, Vector4 rect)
        {
            canvas.DrawBitmap(source, SKRect.Create(rect.X, rect.Y, rect.Z, rect.W));
        }

        public void DrawCircle(Vector2 center, float radius, SKPaint paint)
        {
            canvas.DrawCircle(center.X, center.Y, radius, paint);
        }

        public void DrawOval(Vector2 center, Vector2 radius, SKPaint paint)
        {
            canvas.DrawOval(center.X, center.Y, radius.X, radius.Y, paint);
        }

        public void DrawPath(SKPath path, SKPaint paint)
        {
            canvas.DrawPath(path, paint);
        }

        public void DrawRect(SKRect rect, SKPaint paint)
        {
            canvas.DrawRect(rect, paint);
        }

        public void DrawRect(SKRoundRect rect, SKPaint paint)
        {
            canvas.DrawRoundRect(rect, paint);
        }

        public void DrawSurface(SKSurface surface)
        {
            DrawSurface(surface, Vector2.Zero);
        }

        public void DrawSurface(SKSurface surface, Vector2 point)
        {
            canvas.DrawSurface(surface, point.X, point.Y);
        }

        public void DrawSurface(SKSurface surface, Vector4 rect)
        {
            canvas.DrawSurface(surface, rect.X, rect.Y);
        }

        public void DrawText(string text, Vector2 point, SKTextAlign textAlign, SKFont font, SKPaint paint)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            canvas.DrawText(text, point.X, point.Y, textAlign, font, paint);
        }

        public void DrawTexture(SKBitmap source, SKPoint[] sourceVertices, SKPoint[] vertices)
        {
            using var paint = new SKPaint()
            {
                IsAntialias = true,
                Shader = SKShader.CreateBitmap(source, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp)
            };
            canvas.DrawVertices(SKVertexMode.TriangleFan,
                vertices,
                sourceVertices, null, paint);
        }
    }
}
