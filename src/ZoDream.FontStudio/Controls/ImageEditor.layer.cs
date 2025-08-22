using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.ImageEditor;

namespace ZoDream.FontStudio.Controls
{
    public partial class ImageEditor : IImageEditor
    {

        private IMouseState? _currentState;

        public Vector2 Size => ActualSize;

        public void Add(IEnumerable<IImageLayer> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(IImageLayer layer)
        {
        }

        public void Clear()
        {
        }

 

        public void Invalidate()
        {
        }

        public void Paint(SKCanvas canvas, SKImageInfo info)
        {
        }

        public void Remove(IImageLayer layer)
        {
        }

        public void Select(IImageLayer? layer)
        {
        }

        public void Tap(float x, float y)
        {
        }

        public void Unselect()
        {
        }

        public void Dispose()
        {
        }
    }
}
