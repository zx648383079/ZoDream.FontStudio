using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface ICommandController : IMouseState, IDisposable
    {

        public void Paint(IImageCanvas canvas);
    }
}
