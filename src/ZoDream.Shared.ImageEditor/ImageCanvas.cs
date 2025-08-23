using SkiaSharp;

namespace ZoDream.Shared.ImageEditor
{
    public class ImageCanvas(SKCanvas canvas) : IImageCanvas
    {
        public void DrawBitmap(SKBitmap source)
        {
            canvas.DrawBitmap(source, SKPoint.Empty);
        }

        public void DrawBitmap(SKBitmap source, SKPoint point)
        {
            DrawBitmap(source, SKRect.Create(point, new SKSize(source.Width, source.Height)));
        }

        public void DrawBitmap(SKBitmap source, SKRect rect)
        {
            canvas.DrawBitmap(source, rect);
        }

        public void DrawCircle(SKPoint center, float radius, SKPaint paint)
        {
            canvas.DrawCircle(center, radius, paint);
        }

        public void DrawOval(SKPoint center, SKSize radius, SKPaint paint)
        {
            canvas.DrawOval(center, radius, paint);
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
            DrawSurface(surface, SKPoint.Empty);
        }

        public void DrawSurface(SKSurface surface, SKPoint point)
        {
            canvas.DrawSurface(surface, point);
        }

        public void DrawSurface(SKSurface surface, SKRect rect)
        {
            canvas.DrawSurface(surface, rect.Left, rect.Top);
        }

        public void DrawText(string text, SKPoint point, SKTextAlign textAlign, SKFont font, SKPaint paint)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            canvas.DrawText(text, point, textAlign, font, paint);
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
