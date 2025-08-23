using SkiaSharp;

namespace ZoDream.Shared.ImageEditor
{
    public interface IMouseState
    {

        public void PointerPressed(SKPoint point);
        public void PointerMoved(SKPoint point);
        public void PointerReleased();
    }
}
