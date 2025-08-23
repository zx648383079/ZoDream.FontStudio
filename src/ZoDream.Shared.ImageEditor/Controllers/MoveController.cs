using SkiaSharp;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class MoveController(IImageEditor editor) : ICommandController
    {

        private SKPoint _start = SKPoint.Empty;
        private PointerState _pointerState = PointerState.Released;
        private IMouseState? _currentState;
        private ICommandLayer? _currentCommand;

        public void Touch(SKPoint point)
        {

        }

        public void PointerMoved(SKPoint point)
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
            editor.Invalidate();
        }

        public void PointerPressed(SKPoint point)
        {
            _start = point;
            _pointerState = PointerState.Pressed;
        }

        public void PointerReleased()
        {
            var state = _pointerState;
            _pointerState = PointerState.Released;
            if (state != PointerState.Moved)
            {
                Touch(_start);
                return;
            }
            _currentState?.PointerReleased();
            editor.Invalidate();
        }

        public void Paint(IImageCanvas canvas)
        {
            _currentCommand?.Paint(canvas);
        }

        public void Dispose()
        {
            _currentCommand?.Dispose();
        }


    }
}
