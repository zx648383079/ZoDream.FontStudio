using SkiaSharp;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor.Layers
{
    public class TransparentLayer : IImageSource, ICommandLayer
    {
        public TransparentLayer(IImageEditor editor)
        {
            _editor = editor;
            Invalidate();
        }

        private readonly IImageEditor _editor;
        private readonly int _gridSize = 10;

        private SKSurface? _surface;

        public void Resize(Vector2 size)
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

        private void RenderSurface()
        {
            var size = _editor.Size;
            if (size.X == 0 || size.Y == 0)
            {
                return;
            }
            var info = new SKImageInfo((int)size.X, (int)size.Y);
            _surface = SKSurface.Create(info);
            var canvas = _surface.Canvas;
            canvas.Clear(SKColors.White);
            using var grayPaint = new SKPaint()
            {
                Color = SKColors.LightGray,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 0,
            };
            var columnCount = size.X / _gridSize + 1;
            var rowCount = size.Y / _gridSize + 1;
            for (var i = 0; i < columnCount; i++)
            {
                for (var j = 0; j < rowCount; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        continue;
                    }
                    canvas.DrawRect(i * _gridSize, j * _gridSize, _gridSize, _gridSize, grayPaint);
                }
            }
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
