using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.ImageEditor
{
    public interface ICommandLayer
    {
        public void Resize(Vector2 size);
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
