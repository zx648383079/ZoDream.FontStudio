using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor.Sources
{
    public class PathImageSource : IImageSource
    {

        private readonly SKPaint _paint = new()
        {
            Color = SKColors.Black,
            ColorF = SKColors.Black,
            StrokeWidth = 2,
            IsStroke = true,
            IsAntialias = true,
        };
        private readonly SKPath _path = new();
        public SKRect Bound => _path.Bounds;

        public bool IsEmpty => _path.IsEmpty;
        public SKPoint Last => _path.LastPoint;

        public SKPoint[] Points => _path.Points;

        public bool Contains(SKPoint point)
        {
            return Bound.Contains(point);
        }

        public SKBitmap? CreateThumbnail(SKSize size)
        {
            return null;
        }

        public void Add(SKPoint point)
        {
            if (IsEmpty)
            {
                _path.MoveTo(point);
                return;
            }
            _path.LineTo(point);
        }

        public void ClosePath()
        {
            _path.Close();
            _paint.IsStroke = false;
        }

        public int NearOf(SKPoint point, float maxOffset)
        {
            for (int i = _path.PointCount - 1; i >= 0; i--)
            {
                var offset = point - _path.Points[i];
                if (Math.Abs(offset.X) < maxOffset && Math.Abs(offset.Y) < maxOffset)
                {
                    return i;
                }
            }
            return -1;
        }

        public int IndexOf(SKPoint point)
        {
            for (int i = _path.PointCount - 1; i >= 0; i--)
            {
                if (_path.Points[i] == point)
                {
                    return i;
                }
            }
            return -1;
        }

        public SKPoint Get(int index)
        {
            return _path.GetPoint(index);
        }

        public void RemoveAt(int index)
        {
            
        }

        public void MoveTo(int index, SKPoint to)
        {
            _path.Points[index] = to;
        }

        public void Paint(IImageCanvas canvas)
        {
            canvas.DrawPath(_path, _paint);
        }

        public void Dispose()
        {
            _paint.Dispose();
            _path.Dispose();
        }
    }
}
