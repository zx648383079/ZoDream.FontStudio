using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.FontStudio.Controls;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.ImageEditor.Controllers;

namespace ZoDream.FontStudio.ViewModels
{
    public partial class WorkspaceViewModel : ObservableObject, IDisposable
    {
        public WorkspaceViewModel()
        {
            ModeCommand = new RelayCommand<string>(TapMode);
            ExitCommand = new RelayCommand(TapExit);
        }

        public void Initialize(IImageEditor editor)
        {
            Instance = editor;
            editor.SwitchMode<MoveController>();
            if (editor is ImageEditor e)
            {
                e.Layer = this;
            }
        }

        public void Dispose()
        {
            Instance?.Dispose();
        }
    }
}
