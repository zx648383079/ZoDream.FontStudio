using SkiaSharp;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.ImageEditor.Layers;

namespace ZoDream.FontStudio.Controls
{
    public partial class ImageEditor : IImageEditor
    {


        private TransparentLayer? _backgroundLayer;
        public ILayerController? Layer { get; set; }
        public ICommandController? Command { get; set; }

        public SKSize Size { get; private set; } = SKSize.Empty;

        public IImageLayer? Current => Layer?.Current;

        public void Resize(SKSize size)
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
            Layer?.Paint(c);
            Command?.Paint(c);
        }

        public void Select(IImageLayer? layer)
        {
        }

        public void Touch(SKPoint point)
        {
            
        }

        public void Unselect()
        {
            Invalidate();
        }

        public void Dispose()
        {
            _backgroundLayer?.Dispose();
            Command?.Dispose();
        }

        private void OnPointerPressed(SKPoint point)
        {
            Command?.PointerPressed(point);
        }

        private void OnPointerMoved(SKPoint point)
        {
            Command?.PointerMoved(point);
        }

        private void OnPointerReleased()
        {
            Command?.PointerReleased();
        }

        public void Add(IImageSource source)
        {
            throw new System.NotImplementedException();
        }
    }
}
