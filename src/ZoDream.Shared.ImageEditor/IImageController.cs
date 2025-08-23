using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageController : IEnumerable<IImageSource>
    {
        public void Add(IEnumerable<IImageLayer> items);

        public void Add(IImageLayer layer);

        /// <summary>
        /// 移除图层并销毁
        /// </summary>
        /// <param name="layer"></param>
        public void Remove(IImageLayer layer);

        /// <summary>
        /// 清除全部图层
        /// </summary>
        public void Clear();
        public bool TryGet(Vector2 point, out IImageLayer? layer);

        public IEnumerable<IImageLayer> Get(Vector4 rect);
    }
}
