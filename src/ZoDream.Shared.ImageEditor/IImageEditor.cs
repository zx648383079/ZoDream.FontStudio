using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageEditor : IDisposable
    {
        public Vector2 Size { get; }

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

        public void Tap(float x, float y);

        public void Select(IImageLayer? layer);

        public void Unselect();

        public void Paint(SKCanvas canvas, SKImageInfo info);

        public void Invalidate();
    }
}
