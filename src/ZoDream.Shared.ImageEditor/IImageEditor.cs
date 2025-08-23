using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageEditor : IDisposable
    {
        public SKSize Size { get; }

        public IImageLayer? Current { get; }

        public void Add(IImageSource source);

        public void Touch(SKPoint point);

        public void Select(IImageLayer? layer);

        public void Unselect();

        public void Paint(SKCanvas canvas, SKImageInfo info);

        public void Invalidate();
    }
}
