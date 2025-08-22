using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageLayer : IDisposable
    {
        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public bool IsLocked { get; set; }

        public int Depth { get; set; }

        public IImageSource Source { get; set; }

        /// <summary>
        /// 源更新了，需要重采样
        /// </summary>
        public void Resample();

        public void Paint(IImageCanvas canvas);
    }
}
