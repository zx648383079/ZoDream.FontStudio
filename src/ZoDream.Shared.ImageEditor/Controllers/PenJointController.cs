using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.ImageEditor.Layers;
using ZoDream.Shared.ImageEditor.Sources;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class PenJointController(IImageEditor editor) : ICommandController, IMouseState
    {
        private IMouseState? _currentState;
        private PathImageSource? _layer;
        private IList<int> _selected = [];
        public bool IsEnabled => editor.Current?.Source is PathImageSource;

        public void Initialize(IImageLayer? layer)
        {
            _selected = [];
            if (layer?.Source is PathImageSource p)
            {
                _layer = p;
                return;
            }
            _layer = null;
        }

        public void Touch(SKPoint point)
        {
            editor.Invalidate();
        }


        public void PointerPressed(IMouseRoutedArgs args)
        {
            if (_layer is null)
            {
                return;
            }
            var index = _layer.NearOf(args.Position, editor.Options.JointSize / 2);
            if (index >= 0)
            {
                _selected = [index];
            }
            if (index < 0 && args.IsLeftButtonPressed)
            {
                _currentState ??= new SelectLayer();
                _currentState?.PointerPressed(args);
            }
        }

        public void PointerMoved(IMouseRoutedArgs args)
        {
            if (_layer is null && args.State.HasFlag(PointerState.Released))
            {
                return;
            }
            if (_selected.Count > 0)
            {
                _layer?.MoveTo(_selected[0], args.Position);
            } else
            {
                _currentState?.PointerMoved(args);
            }
            editor.Invalidate();
        }

        public void PointerReleased(IMouseRoutedArgs args)
        {
            if (args.State.HasFlag(PointerState.NotMoved))
            {
                Touch(args.Position);
                return;
            }
            if (_currentState is SelectLayer s && _layer is not null && s.IsVisible)
            {
                var rect = s.Bound;
                _selected.Clear();
                for (var i = 0; i < _layer.Points.Length; i++)
                {
                    if (rect.Contains(_layer.Points[i]))
                    {
                        _selected.Add(i);
                    }
                }
            }
            _currentState?.PointerReleased(args);
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
                    _selected.Contains(i) ? options.JointHoveredPaint : options.JointPaint);
                canvas.DrawRect(joint, options.JointStrokePaint);
            }
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
