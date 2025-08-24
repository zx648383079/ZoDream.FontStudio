using SkiaSharp;
using System;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.FontStudio.Controls
{
    public partial class ImageEditor : IImageEditor
    {
        private TransparentLayer? _backgroundLayer;
        public ILayerController? Layer { get; set; }
        public ICommandController? Command { get; set; }

        public IImageOptions Options { get; private set; } = new DefaultImageOptions();

        public SKSize Size { get; private set; } = SKSize.Empty;

        public IImageLayer? Current => Layer?.Current;

        public void Resize(SKSize size)
        {
            Size = size;
            ResizeWithControl(size);
            _backgroundLayer?.Invalidate();
        }

        public void SwitchMode<T>() where T : ICommandController
        {
            if (Command is T)
            {
                return;
            }
            Command?.Dispose();
            Command = (T)Activator.CreateInstance(typeof(T), this);
            Command?.Initialize(Current);
            Invalidate();
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
            Layer?.Paint(c);
            Command?.Paint(c);
        }

        public void Select(IImageLayer? layer)
        {
            Command?.Initialize(layer);
        }

        public void Touch(SKPoint point)
        {
            
        }

        public void Unselect()
        {
            Invalidate();
        }

   

        private void OnPointerPressed(IMouseRoutedArgs args)
        {
            if (Command is IMouseState t)
            {
                t.PointerPressed(args);
            }
        }

        private void OnPointerMoved(IMouseRoutedArgs args)
        {
            if (Command is IMouseState t)
            {
                t.PointerMoved(args);
            }
        }

        private void OnPointerReleased(IMouseRoutedArgs args)
        {
            if (Command is IMouseState t)
            {
                t.PointerReleased(args);
            }
        }

        private void OnKeyPressed(IKeyboardRoutedArgs args)
        {
            if (Command is IKeyboardState t)
            {
                t.KeyPressed(args);
            }
        }

        private void OnKeyReleased(IKeyboardRoutedArgs args)
        {
            if (Command is IKeyboardState t)
            {
                t.KeyReleased(args);
            }
        }

        public void Add(IImageSource source)
        {
            Layer?.Add(source);
        }

        public void Dispose()
        {
            _backgroundLayer?.Dispose();
            Command?.Dispose();
            Options?.Dispose();
        }
    }
}
