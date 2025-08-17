using Microsoft.UI.Xaml;
using ZoDream.FontStudio.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.FontStudio
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel.Binding(this, RootFrame, RootMenuBar);
        }

        internal AppViewModel ViewModel => App.ViewModel;

    }
}
