using SkiaSharp;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor.Layers
{
    public class HighlightLayer : IImageSource, ICommandLayer
    {
        public HighlightLayer(IImageEditor editor)
        {
            _editor = editor;
        }

        private readonly IImageEditor _editor;
        private SKSurface? _surface;
        private IImageLayer? _target;

        public Vector4 Bound => Vector4.Zero;

        public void Resize(Vector2 size)
        {
            Invalidate();
        }

        public void With(IImageLayer layer)
        {
            _target = layer;
            SyncSize();
            Invalidate();
        }

        public void Invalidate()
        {
            _surface?.Dispose();
            _surface = null;
        }

        private void SyncSize()
        {
            if (_target is null)
            {
                return;
            }
        }

        private void RenderSurface()
        {
            SyncSize();
            var size = _editor.Size;
            var info = new SKImageInfo((int)size.X, (int)size.Y);
            _surface = SKSurface.Create(info);
            var canvas = _surface.Canvas;
            canvas.Clear(SKColors.Transparent);
            using var paint = new SKPaint()
            {
                Color = new SKColor(0, 0, 0, 150),
                Style = SKPaintStyle.Fill,
                StrokeWidth = 0,
            };
            var bound = _target.Source.Bound;
            if (bound.X > 0)
            {
                canvas.DrawRect(0, 0, bound.X, info.Height, paint);
            }
            var right = bound.X + bound.Z;
            if (right < info.Width)
            {
                canvas.DrawRect(right, 0, info.Width - right, info.Height, paint);
            }
            if (bound.Y > 0)
            {
                canvas.DrawRect(bound.X, 0, bound.Z, bound.Y, paint);
            }
            var bottom = bound.Y + bound.Z;
            if (bottom < info.Height)
            {
                canvas.DrawRect(bound.X, bottom, bound.Z, info.Height - bottom, paint);
            }
        }

        public void Paint(IImageCanvas canvas)
        {
            if (_surface == null)
            {
                RenderSurface();
            }
            canvas.DrawSurface(_surface);
        }

        public void Dispose()
        {
            _surface?.Dispose();
        }

        public bool Contains(Vector2 point)
        {
            return false;
        }

        public SKBitmap? CreateThumbnail(Vector2 size)
        {
            return null;
        }
    }
}
