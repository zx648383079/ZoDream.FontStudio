using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface ICommandController : IDisposable
    {

        public bool IsEnabled { get; }

        public void Initialize(IImageLayer? layer);

        public void Paint(IImageCanvas canvas);
    }
}
