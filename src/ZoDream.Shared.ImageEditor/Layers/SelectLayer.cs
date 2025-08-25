using SkiaSharp;
using System;
using System.Numerics;

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
        private bool _isEnabled;

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

        public void Invalidate()
        {
        }

        public void Paint(IImageCanvas canvas)
        {
            canvas.DrawRect(new SKRect(
                Math.Min(_start.X, _last.X), Math.Min(_start.Y, _last.Y),
                Math.Max(_start.X, _last.X), Math.Max(_start.Y, _last.Y)
                ), 
                _paint);
        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
            if (!_isEnabled)
            {
                return;
            }
            _last = args.Position;
        }

        public void PointerPressed(IMouseRoutedArgs args)
        {
            _isEnabled = true;
            _start = args.Position;
            _last = args.Position;
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            _isEnabled = false;
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
