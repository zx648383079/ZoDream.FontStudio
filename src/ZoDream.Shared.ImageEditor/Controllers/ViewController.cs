using SkiaSharp;

namespace ZoDream.Shared.ImageEditor.Controllers
{
    public class ViewController(IImageEditor editor) : ICommandController
    {
        public bool IsEnabled => true;

        public void Initialize(IImageLayer? layer)
        {
        }


        public void Paint(IImageCanvas canvas)
        {
        }

        public void Dispose()
        {
        }


    }
}
