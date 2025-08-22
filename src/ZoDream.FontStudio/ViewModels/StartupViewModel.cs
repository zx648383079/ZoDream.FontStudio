using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.FontStudio.Pages;

namespace ZoDream.FontStudio.ViewModels
{
    public class StartupViewModel : ObservableObject
    {
        public StartupViewModel()
        {
            OpenCommand = new RelayCommand(TapOpen);
            CreateCommand = new RelayCommand(TapCreate);
            version = App.ViewModel.Version;
        }

        private string version;

        public string Version {
            get => version;
            set => SetProperty(ref version, value);
        }

        public ICommand OpenCommand { get; private set; }

        public ICommand CreateCommand { get; private set; }

        private async void TapOpen()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*.ttf");
            picker.FileTypeFilter.Add("*.otf");
            picker.FileTypeFilter.Add("*.woff");
            picker.FileTypeFilter.Add("*.woff2");
            App.ViewModel.InitializePicker(picker);
            var items = await picker.PickMultipleFilesAsync();
            if (items.Count == 0)
            {
                return;
            }
            App.ViewModel.Navigate<WorkspacePage>(items);
        }

        private void TapCreate()
        {
            App.ViewModel.Navigate<WorkspacePage>();
        }
    }
}
