using SkiaSharp;
using System;
using System.Collections.Generic;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.FontStudio.Controls
{
    public partial class ImageEditor : IImageEditor
    {
        private readonly IList<ICommandLayer> _bottomLayers = [];
        private readonly IList<ICommandLayer> _topLayers = [];
        public ILayerController? Layer { get; set; }
        public ICommandController? Command { get; set; }

        public IImageOptions Options { get; private set; } = new DefaultImageOptions();

        public SKSize Size { get; private set; } = SKSize.Empty;

        public IImageLayer? Current => Layer?.Current;

        public void Initialize()
        {
            _bottomLayers.Add(new TransparentLayer(this));
            _bottomLayers.Add(new GlyphLayoutLayer(this));
        }

        public void Resize(SKSize size)
        {
            Size = size;
            ResizeWithControl(size);
            foreach (var layer in _bottomLayers)
            {
                layer.Invalidate();
            }
            foreach (var layer in _topLayers)
            {
                layer.Invalidate();
            }
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
            foreach (var layer in _bottomLayers)
            {
                if (!layer.IsVisible)
                {
                    continue;
                }
                layer.Paint(c);
            }
            Layer?.Paint(c);
            Command?.Paint(c);
            foreach (var layer in _topLayers)
            {
                if (!layer.IsVisible)
                {
                    continue;
                }
                layer.Paint(c);
            }
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
            Command?.Dispose();
            foreach (var layer in _bottomLayers)
            {
                layer.Dispose();
            }
            foreach (var layer in _topLayers)
            {
                layer.Dispose();
            }
            Options?.Dispose();
        }
    }
}
