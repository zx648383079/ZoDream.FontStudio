using SkiaSharp;

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

        public SKRect Bound => SKRect.Empty;

        public void Resize(SKSize size)
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
            var info = new SKImageInfo((int)size.Width, (int)size.Height);
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
            if (bound.Left > 0)
            {
                canvas.DrawRect(0, 0, bound.Left, info.Height, paint);
            }
            if (bound.Right < info.Width)
            {
                canvas.DrawRect(bound.Right, 0, info.Width - bound.Right, info.Height, paint);
            }
            if (bound.Top > 0)
            {
                canvas.DrawRect(bound.Left, 0, bound.Width, bound.Top, paint);
            }
            if (bound.Bottom < info.Height)
            {
                canvas.DrawRect(bound.Left, bound.Bottom, bound.Width, info.Height - bound.Bottom, paint);
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

        public bool Contains(SKPoint point)
        {
            return false;
        }

        public SKBitmap? CreateThumbnail(SKSize size)
        {
            return null;
        }
    }
}
