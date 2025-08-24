using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageEditor : IDisposable
    {
        public IImageOptions Options { get; }

        public SKSize Size { get; }

        public IImageLayer? Current { get; }
        /// <summary>
        /// 切换模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SwitchMode<T>() where T : ICommandController;

        public void Add(IImageSource source);

        public void Touch(SKPoint point);

        public void Select(IImageLayer? layer);

        public void Unselect();

        public void Paint(SKCanvas canvas, SKImageInfo info);

        public void Invalidate();
    }
}
