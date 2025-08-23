using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using ZoDream.Shared.ImageEditor;

namespace ZoDream.FontStudio.ViewModels
{
    public class ImageLayerViewModel(WorkspaceViewModel workspace, IImageSource source) : ObservableObject, IImageLayer
    {

        public WorkspaceViewModel Workspace => workspace;

        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private BitmapSource? _previewImage;

        public BitmapSource? PreviewImage {
            get => _previewImage;
            set => SetProperty(ref _previewImage, value);
        }

        private bool _isVisible = true;

        public bool IsVisible {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private bool _isLocked;

        public bool IsLocked {
            get => _isLocked;
            set => SetProperty(ref _isLocked, value);
        }

        public int Depth { get; set; }

        public IImageSource Source { get; set; } = source;


        public void Resample()
        {
            PreviewImage = Workspace.CreateThumbnail(Source);
        }

        public void Paint(IImageCanvas canvas)
        {
            if (IsVisible)
            {
                Source?.Paint(canvas);
            }
        }


        public void Dispose()
        {
            Source?.Dispose();
        }
    }
}
