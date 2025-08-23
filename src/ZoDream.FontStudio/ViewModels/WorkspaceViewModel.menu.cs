using System.Collections.Generic;
using System.Windows.Input;
using Windows.Storage;

namespace ZoDream.FontStudio.ViewModels
{
    public partial class WorkspaceViewModel
    {

        

        public ICommand ExitCommand { get; private set; }

        private void TapExit(object? _)
        {
            App.Current.Exit();
        }

    }
}
