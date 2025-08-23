using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageEditor : IDisposable
    {
        public Vector2 Size { get; }

        public void Touch(Vector2 point);

        public void Select(IImageLayer? layer);

        public void Unselect();

        public void Paint(SKCanvas canvas, SKImageInfo info);

        public void Invalidate();
    }
}
