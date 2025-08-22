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
        private Vector2 _start = Vector2.Zero;
        private Vector2 _last = Vector2.Zero;
        private bool _isEnabled;
        public Vector4 Bound => Vector4.Zero;

        public bool Contains(Vector2 point)
        {
            return false;
        }

        public SKBitmap? CreateThumbnail(Vector2 size)
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

        public void PointerMoved(Vector2 point)
        {
            if (!_isEnabled)
            {
                return;
            }
            _last = point;
        }

        public void PointerPressed(Vector2 point)
        {
            _isEnabled = true;
            _start = point;
            _last = point;
        }

        public void PointerReleased()
        {
            _isEnabled = false;
        }

        public void Resize(Vector2 size)
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
