using SkiaSharp;
using System;
using ZoDream.Shared.ImageEditor.Sources;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class PenController(IImageEditor editor) : ICommandController, IMouseState
    {
        public bool IsEnabled => true;
        private bool _isRightButtonPressed = false;
        private SKPoint _last = SKPoint.Empty;
        private readonly SKPaint _paint = new()
        {
            Color = SKColors.Blue.WithAlpha(150),
            StrokeWidth = 1,
            IsStroke = true
        };
        private PathImageSource? _layer;


        public void Initialize(IImageLayer? layer)
        {
            if (layer?.Source is PathImageSource p)
            {
                _layer = p;
                return;
            }
            _layer = null;
        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
            _last = args.Position;
            if (_layer is null || _layer.IsEmpty)
            {
                return;
            }
            editor.Invalidate();
        }

        public void PointerPressed(IMouseRoutedArgs args)
        {
            _isRightButtonPressed = args.IsRightButtonPressed;
            _last = args.Position;
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            if (_isRightButtonPressed)
            {
                _layer = null;
                editor.Invalidate();
                return;
            }
            if (_layer is null)
            {
                editor.Add(_layer = new PathImageSource());
            }
            if (IsClosePath(_last))
            {
                _layer.ClosePath();
                _layer = null;
            } else
            {
                _layer.Add(_last);
            }
            editor.Invalidate();
        }

        private bool IsClosePath(SKPoint point)
        {
            if (_layer!.Points.Length <= 2)
            {
                return false;
            }
            var offset = point - _layer.Points[0];
            var maxOffset = editor.Options.JointSize / 2;
            return Math.Abs(offset.X) < maxOffset && Math.Abs(offset.Y) < maxOffset;
        }


        public void Paint(IImageCanvas canvas)
        {
            if (_layer is null || _layer.IsEmpty)
            {
                return;
            }
            canvas.DrawLine(_layer.Last, _last, _paint);
        }

        public void Dispose()
        {
            _paint?.Dispose();
        }

    }
}
