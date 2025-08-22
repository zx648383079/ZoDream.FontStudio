using System.Numerics;

namespace ZoDream.Shared.ImageEditor
{
    public interface IMouseState
    {

        public void PointerPressed(Vector2 point);
        public void PointerMoved(Vector2 point);
        public void PointerReleased();
    }
}
