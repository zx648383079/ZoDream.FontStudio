using SkiaSharp;
using ZoDream.Shared.ImageEditor.Sources;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class PenJointController(IImageEditor editor) : ICommandController, IMouseState
    {
        private PointerState _pointerState = PointerState.Released;
        private PathImageSource? _layer;
        private int _selected = -1;
        public bool IsEnabled => editor.Current?.Source is PathImageSource;

        public void Initialize(IImageLayer? layer)
        {
            _selected = -1;
            if (layer?.Source is PathImageSource p)
            {
                _layer = p;
                return;
            }
            _layer = null;
        }

        

        public void PointerPressed(IMouseRoutedArgs args)
        {
            _pointerState = PointerState.Pressed;
            if (_layer is null)
            {
                return;
            }
            _selected = _layer.NearOf(args.Position, editor.Options.JointSize / 2);
            editor.Invalidate();
        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
            if (_layer is null && _pointerState != PointerState.Pressed)
            {
                return;
            }
            if (_selected >= 0)
            {
                _layer?.MoveTo(_selected, args.Position);
                editor.Invalidate();
            }
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            _pointerState = PointerState.Released;
            editor.Invalidate();
        }

        public void Paint(IImageCanvas canvas)
        {
            if (_layer is null || _layer.IsEmpty)
            {
                return;
            }
            var options = editor.Options;
            var jointSize = options.JointSize;
            var jointHalf = jointSize / 2;
            for (int i = 0; i < _layer.Points.Length; i++)
            {
                var item = _layer.Points[i];
                var joint = SKRect.Create(item.X - jointHalf, item.Y - jointHalf, jointSize, jointSize);
                canvas.DrawRect(joint,
                    i == _selected ? options.JointHoveredPaint : options.JointPaint);
                canvas.DrawRect(joint, options.JointStrokePaint);
            }
        }

        public void Dispose()
        {
        }

        
    }
}
