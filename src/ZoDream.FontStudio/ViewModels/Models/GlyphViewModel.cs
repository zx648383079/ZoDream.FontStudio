using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ZoDream.FontStudio.ViewModels
{
    public class GlyphViewModel : ObservableObject
    {
        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }

    public class GlyphGroupViewModel : ObservableObject
    {
        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<GlyphViewModel> _items = [];

        public ObservableCollection<GlyphViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

    }
}
