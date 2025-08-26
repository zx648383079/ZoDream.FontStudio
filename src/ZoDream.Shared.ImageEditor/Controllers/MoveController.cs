using SkiaSharp;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class MoveController(IImageEditor editor) : ICommandController, IMouseState
    {
        private IMouseState? _currentState;

        public bool IsEnabled => true;

        public void Initialize(IImageLayer? layer)
        {
        }

        public void Touch(SKPoint point)
        {

        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
            if (args.State.HasFlag(PointerState.Released) || _currentState is null)
            {
                return;
            }
            _currentState?.PointerMoved(args);
            editor.Invalidate();
        }

        public void PointerPressed(IMouseRoutedArgs args)
        {
            if (!args.IsLeftButtonPressed)
            {
                return;
            }
            _currentState ??= new SelectLayer();
            _currentState?.PointerPressed(args);
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            if (args.State.HasFlag(PointerState.NotMoved))
            {
                Touch(args.Position);
                return;
            }
            _currentState?.PointerReleased(args);
            editor.Invalidate();
        }

        public void Paint(IImageCanvas canvas)
        {
            if (_currentState is ICommandLayer layer)
            {
                layer?.Paint(canvas);
            }
        }

        public void Dispose()
        {
            if (_currentState is ICommandLayer layer)
            {
                layer?.Dispose();
            }
        }


    }
}
