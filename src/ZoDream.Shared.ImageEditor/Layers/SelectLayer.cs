using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor.Layers
{
    public class SelectLayer : IImageSource, ICommandLayer, IMouseState
    {

        private readonly SKPaint _paint = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 1,
            Style = SKPaintStyle.StrokeAndFill,
            ColorF = SKColors.Blue.WithAlpha(50)
        };
        private SKPoint _start = SKPoint.Empty;
        private SKPoint _last = SKPoint.Empty;

        public bool IsVisible { get; set; } = false;
        public SKRect Bound => new(
                Math.Min(_start.X, _last.X), Math.Min(_start.Y, _last.Y),
                Math.Max(_start.X, _last.X), Math.Max(_start.Y, _last.Y)
                );

        public bool Contains(SKPoint point)
        {
            return false;
        }

        public SKBitmap? CreateThumbnail(SKSize size)
        {
            return null;
        }

        public void Invalidate()
        {
        }

        public void Paint(IImageCanvas canvas)
        {
            if (!IsVisible)
            {
                return;
            }
            canvas.DrawRect(Bound, 
                _paint);
        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
            if (!IsVisible)
            {
                return;
            }
            _last = args.Position;
        }

        public void PointerPressed(IMouseRoutedArgs args)
        {
            IsVisible = true;
            _start = args.Position;
            _last = args.Position;
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            IsVisible = false;
        }

        public void Resize(SKSize size)
        {
        }

        public void With(IImageLayer layer)
        {
        }

        public void Dispose()
        {
            _paint.Dispose();
        }
    }
}
