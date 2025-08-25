using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface ICommandLayer : IDisposable
    {
        public bool IsVisible { get; set; }

        public void Resize(SKSize size);
        /// <summary>
        /// 设置面向的图层
        /// </summary>
        /// <param name="layer"></param>
        public void With(IImageLayer layer);

        /// <summary>
        /// 依赖Editor尺寸的需要重绘
        /// </summary>
        public void Invalidate();

        public void Paint(IImageCanvas canvas);
    }
}
