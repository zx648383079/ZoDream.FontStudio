using SkiaSharp;

namespace ZoDream.Shared.ImageEditor.Layers
{
    public class ResizeLayer : IImageSource, ICommandLayer, IMouseState
    {

        private readonly int _jointSize = 24;
        private readonly SKPaint _paint = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 1,
            Style = SKPaintStyle.StrokeAndFill,
            ColorF = SKColors.Blue.WithAlpha(50)
        };
        private readonly SKPaint _jointPaint = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 1,
            Style = SKPaintStyle.StrokeAndFill,
            ColorF = SKColors.White
        };
        private readonly SKPaint _hoveredPaint = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 1,
            Style = SKPaintStyle.StrokeAndFill,
            ColorF = SKColors.Blue
        };
        private SKSurface? _surface;
        public SKRect Bound { get; private set; } = SKRect.Empty;

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
            var bound = Bound;
            var info = new SKImageInfo((int)bound.Width, (int)bound.Height);
            _surface = SKSurface.Create(info);
            var canvas = _surface.Canvas;
            canvas.DrawRect(bound, _paint);
            var jointHalf = (float)_jointSize / 2;
            var jointX = bound.Left - jointHalf;
            var jointY = bound.Top - jointHalf;
            var widthHalf = bound.Width / 2;
            var heightHalf = bound.Height / 2;
            for (int i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        continue;
                    }
                    canvas.DrawRect(SKRect.Create(jointX + i * widthHalf, 
                        jointY + j * heightHalf, _jointSize, _jointSize), _jointPaint);
                }
            }
        }

        public void PointerMoved(SKPoint point)
        {
        }

        public void PointerPressed(SKPoint point)
        {
        }

        public void PointerReleased()
        {
        }

        public void Resize(SKSize size)
        {
        }

        public void With(IImageLayer layer)
        {
            Bound = layer.Source.Bound;
        }

        public void Dispose()
        {
            _surface?.Dispose();
        }
    }
}
