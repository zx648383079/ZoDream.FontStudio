using System.Collections.ObjectModel;
using ZoDream.FontStudio.Controls;
using ZoDream.Shared.UndoRedo;

namespace ZoDream.FontStudio.ViewModels
{
    public partial class WorkspaceViewModel
    {

        private readonly AppViewModel _app = App.ViewModel;

        public CommandManager UndoRedo { get; private set; } = new();
        public ImageEditor? Instance { get; set; }


        private bool _undoEnabled;

        public bool UndoEnabled {
            get => _undoEnabled;
            set => SetProperty(ref _undoEnabled, value);
        }

        private bool _redoEnabled;

        public bool RedoEnabled {
            get => _redoEnabled;
            set => SetProperty(ref _redoEnabled, value);
        }

        private bool _isLoading;

        public bool IsLoading {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private ObservableCollection<object> _glyphItems = [];

        public ObservableCollection<object> GlyphItems {
            get => _glyphItems;
            set => SetProperty(ref _glyphItems, value);
        }



        public bool IsSelectedLayer => SelectedLayer != null;

        

        private object? _selectedLayer;

        public object? SelectedLayer {
            get => _selectedLayer;
            set {
                SetProperty(ref _selectedLayer, value);
                OnPropertyChanged(nameof(IsSelectedLayer));
            }
        }


        private ObservableCollection<object> _layerItems = [];

        public ObservableCollection<object> LayerItems {
            get => _layerItems;
            set => SetProperty(ref _layerItems, value);
        }


    }
}
