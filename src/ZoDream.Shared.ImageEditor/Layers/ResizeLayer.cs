using SkiaSharp;

namespace ZoDream.Shared.ImageEditor.Layers
{
    public class ResizeLayer(IImageEditor editor) : IImageSource, ICommandLayer, IMouseState
    {


        private readonly SKPaint _paint = new()
        {
            IsStroke = true,
            ColorF = SKColors.Blue.WithAlpha(50),
            IsAntialias = true,
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
            var options = editor.Options;
            var bound = Bound;
            var info = new SKImageInfo((int)bound.Width, (int)bound.Height);
            _surface = SKSurface.Create(info);
            var canvas = _surface.Canvas;
            canvas.DrawRect(bound, _paint);
            var jointHalf = options.JointSize / 2;
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
                        jointY + j * heightHalf, options.JointSize, options.JointSize), options.JointPaint);
                }
            }
        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
        }

        public void PointerPressed(IMouseRoutedArgs args)
        {
        }

        public void PointerReleased(IMouseRoutedArgs args)
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
