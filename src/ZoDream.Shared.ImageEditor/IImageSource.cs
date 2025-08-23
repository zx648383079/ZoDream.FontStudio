using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageSource : IDisposable
    {
        public SKRect Bound { get; }

        public bool Contains(SKPoint point);

        public void Paint(IImageCanvas canvas);
        /// <summary>
        /// 生成预览图
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public SKBitmap? CreateThumbnail(SKSize size);
    }
}
