using SkiaSharp;
using System;
using System.Numerics;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.FontStudio.Controls
{
    public partial class ImageEditor : IImageEditor
    {

        private Vector2 _start = Vector2.Zero;
        private PointerState _pointerState = PointerState.Released;
        private IMouseState? _currentState;
        private ICommandLayer? _currentCommand;
        private TransparentLayer? _backgroundLayer;
        public IImageController? Controller { get; set; }

        public Vector2 Size { get; private set; } = Vector2.Zero;

        public void Resize(Vector2 size)
        {
            Size = size;
            ResizeWithControl(size);
            _backgroundLayer?.Invalidate();
        }

        public void Invalidate()
        {
            CanvasTarget.Invalidate();
        }

        public void Paint(SKCanvas canvas, SKImageInfo info)
        {
            canvas.Clear(SKColors.Transparent);
            var c = new ImageCanvas(canvas);
            _backgroundLayer ??= new TransparentLayer(this);
            _backgroundLayer.Paint(c);
            if (Controller is not null)
            {
                foreach (var item in Controller)
                {
                    item.Paint(c);
                }
            }
            _currentCommand?.Paint(c);
        }

        public void Select(IImageLayer? layer)
        {
        }

        public void Touch(Vector2 point)
        {
            
        }

        public void Unselect()
        {
            if (_currentCommand is null)
            {
                return;
            }
            _currentCommand.Dispose();
            _currentCommand = null;
            Invalidate();
        }

        public void Dispose()
        {
            _backgroundLayer?.Dispose();
            if (_currentState is IDisposable d)
            {
                d.Dispose();
            }
            _currentCommand?.Dispose();
        }

        private void OnPointerPressed(Vector2 point)
        {
            _start = point;
            _pointerState = PointerState.Pressed;
        }

        private void OnPointerMoved(Vector2 point)
        {
            if (_pointerState == PointerState.Released)
            {
                return;
            }
            if (_pointerState != PointerState.Moved)
            {
                if (_currentCommand is null)
                {
                    var layer = new SelectLayer();
                    _currentCommand = layer;
                    _currentState = layer;
                }
                _currentState?.PointerPressed(_start);
                _pointerState = PointerState.Moved;
            }
            _currentState?.PointerMoved(point);
            Invalidate();
        }

        private void OnPointerReleased()
        {
            var state = _pointerState;
            _pointerState = PointerState.Released;
            if (state != PointerState.Moved)
            {
                Touch(_start);
                return;
            }
            _currentState?.PointerReleased();
            Invalidate();
        }
    }
}
