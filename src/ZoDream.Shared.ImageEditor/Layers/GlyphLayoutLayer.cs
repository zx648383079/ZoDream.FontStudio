using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor.Layers
{
    /// <summary>
    /// 显示模具
    /// </summary>
    public class GlyphLayoutLayer : IImageSource, ICommandLayer
    {
        public GlyphLayoutLayer(IImageEditor editor)
        {
            _editor = editor;
            Invalidate();
        }

        private readonly IImageEditor _editor;
        private SKSurface? _surface;
        public bool IsVisible { get; set; } = true;
        public SKRect Bound => SKRect.Empty;

        public bool Contains(SKPoint point)
        {
            return false;
        }

        public SKBitmap? CreateThumbnail(SKSize size)
        {
            return null;
        }

        public void Resize(SKSize size)
        {
            Invalidate();
        }

        public void With(IImageLayer layer)
        {
        }


        public void Invalidate()
        {

            _surface?.Dispose();
            _surface = null;
        }

        public void Paint(IImageCanvas canvas)
        {
            if (_surface == null)
            {
                RenderSurface();
            }
            if (_surface == null)
            {
                return;
            }
            canvas.DrawSurface(_surface);
        }

        private void RenderSurface()
        {
            var size = _editor.Size;
            if (size.Width == 0 || size.Height == 0)
            {
                return;
            }
            var info = new SKImageInfo((int)size.Width, (int)size.Height);
            _surface = SKSurface.Create(info);
            var canvas = _surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var width = Math.Min(400, Math.Min(size.Width, size.Height));
            Paint(canvas, SKRect.Create((size.Width - width) / 2, (size.Height - width) / 2, width, width));
        }

        private void Paint(SKCanvas canvas, SKRect rect)
        {
            var options = _editor.Options;
            var vBearingY = rect.Height / 5;
            var hBearingX = rect.Width / 8;

            var glyphWidth = rect.Width - hBearingX * 3;
            var glyphHeight = rect.Height - vBearingY * 1.5f;

            var hBearingY = glyphHeight / 1.5f;
            using var outlinePaint = new SKPaint()
            {
                IsStroke = true,
                StrokeWidth = 2,
                Color = options.Foreground,
                
            };
            using var linePaint = new SKPaint()
            {
                IsStroke = true,
                StrokeWidth = 2,
                Color = options.Foreground.WithAlpha(40),
                PathEffect = SKPathEffect.CreateDash([10, 5], 0)
            };
            DrawVLine(canvas, new SKPoint(rect.Left + hBearingX, rect.Top), rect.Height, linePaint);
            DrawVLine(canvas, new SKPoint(rect.Right - hBearingX * 2, rect.Top), rect.Height, linePaint);

            DrawHLine(canvas, new SKPoint(rect.Left, rect.Top + vBearingY + hBearingY), rect.Width, outlinePaint);
            DrawVLine(canvas, new SKPoint(rect.Left + hBearingX + glyphWidth / 2, rect.Top), rect.Width, outlinePaint);


            DrawHLine(canvas, new SKPoint(rect.Left, rect.Top + vBearingY), rect.Width, linePaint);
            DrawHLine(canvas, new SKPoint(rect.Left, rect.Bottom - vBearingY / 2), rect.Width, linePaint);




            canvas.DrawRect(rect, outlinePaint);
        }

        private void DrawHLine(SKCanvas canvas, SKPoint point, float length, SKPaint paint)
        {
            canvas.DrawLine(point, new SKPoint(point.X + length, point.Y), paint);
        }

        private void DrawVLine(SKCanvas canvas, SKPoint point, float length, SKPaint paint)
        {
            canvas.DrawLine(point, new SKPoint(point.X, point.Y + length), paint);
        }

        public void Dispose()
        {
            _surface?.Dispose();
        }
    }
}
