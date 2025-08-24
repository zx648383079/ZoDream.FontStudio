using SkiaSharp;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class MoveController(IImageEditor editor) : ICommandController, IMouseState
    {

        private IMouseRoutedArgs? _start;
        private PointerState _pointerState = PointerState.Released;
        private IMouseState? _currentState;
        private ICommandLayer? _currentCommand;

        public bool IsEnabled => true;

        public void Initialize(IImageLayer? layer)
        {
        }

        public void Touch(SKPoint point)
        {

        }

        public void PointerMoved(IMouseRoutedArgs args)
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
                _currentState?.PointerPressed(_start!);
                _pointerState = PointerState.Moved;
            }
            _currentState?.PointerMoved(args);
            editor.Invalidate();
        }

        public void PointerPressed(IMouseRoutedArgs args)
        {
            _start = args;
            _pointerState = PointerState.Pressed;
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            _start = null;
            var state = _pointerState;
            _pointerState = PointerState.Released;
            if (state != PointerState.Moved)
            {
                Touch(args.Position);
                return;
            }
            _currentState?.PointerReleased(args);
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
