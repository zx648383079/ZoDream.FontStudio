using SkiaSharp;
using System;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageOptions : IDisposable
    {
        public SKPaint JointStrokePaint { get; }
        public SKPaint JointPaint { get; }
        public SKPaint JointHoveredPaint { get; }
        public float JointSize { get; }

        public SKColor Hovered {  get; }
        public SKColor Activated {  get; }
        public SKColor Background {  get; }
        public SKPaint BackgroundPaint {  get; }
        public SKColor Foreground {  get; }
        public SKPaint ForegroundPaint {  get; }
    }
}
