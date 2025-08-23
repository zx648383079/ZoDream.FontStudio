using SkiaSharp;
using System.Collections.Generic;

namespace ZoDream.Shared.ImageEditor
{
    public interface ILayerController
    {
        public IImageLayer? Current { get; }

        public void Add(IEnumerable<IImageLayer> items);

        public void Add(IImageLayer layer);
        public void Add(IImageSource source);

        /// <summary>
        /// 移除图层并销毁
        /// </summary>
        /// <param name="layer"></param>
        public void Remove(IImageLayer layer);

        /// <summary>
        /// 清除全部图层
        /// </summary>
        public void Clear();
        public bool TryGet(SKPoint point, out IImageLayer? layer);

        public IEnumerable<IImageLayer> Get(SKRect rect);

        public void Paint(IImageCanvas canvas);
    }
}
