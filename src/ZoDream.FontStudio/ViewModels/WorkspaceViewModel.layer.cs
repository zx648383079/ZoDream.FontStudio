using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System.Collections.Generic;
using ZoDream.Shared.ImageEditor;

namespace ZoDream.FontStudio.ViewModels
{
    public partial class WorkspaceViewModel : ILayerController
    {
        private readonly SKSize _thumbnailSize = new(60, 60);

        public IImageLayer? Current => SelectedLayer;

        public void Add(IEnumerable<IImageLayer> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(IImageLayer layer)
        {
            LayerItems.Insert(0, layer);
        }

        public void Add(IImageSource source)
        {
            Add(new ImageLayerViewModel(this, source));
        }

        public void Clear()
        {
            LayerItems.Clear();
        }

        public BitmapSource? CreateThumbnail(IImageSource source)
        {
            return source.CreateThumbnail(_thumbnailSize)?.ToWriteableBitmap();
        }

        public IEnumerable<IImageLayer> Get(SKRect rect)
        {
            foreach (var item in LayerItems)
            {
                if (!item.IsVisible)
                {
                    continue;
                }
                if (rect.IntersectsWith(item.Source.Bound))
                {
                    yield return item;
                }
            }
        }

        public void Paint(IImageCanvas canvas)
        {
            foreach (var item in LayerItems)
            {
                item.Paint(canvas);
            }
        }

        public void Remove(IImageLayer layer)
        {
            LayerItems.Remove(layer);
        }

        public bool TryGet(SKPoint point, out IImageLayer? layer)
        {
            foreach (var item in LayerItems)
            {
                if (item.Source.Bound.Contains(point))
                {
                    layer = item;
                    return true;
                }
            }
            layer = null;
            return false;
        }
    }
}
