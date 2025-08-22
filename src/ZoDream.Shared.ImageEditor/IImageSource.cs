using SkiaSharp;
using System;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageSource : IDisposable
    {
        public Vector4 Bound { get; }

        public bool Contains(Vector2 point);

        public void Paint(IImageCanvas canvas);
        /// <summary>
        /// 生成预览图
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public SKBitmap? CreateThumbnail(Vector2 size);
    }
}
