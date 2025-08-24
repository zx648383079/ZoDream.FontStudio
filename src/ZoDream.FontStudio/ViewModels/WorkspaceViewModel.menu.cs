using System.Collections.Generic;
using System.Windows.Input;
using Windows.Storage;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.ImageEditor.Controllers;

namespace ZoDream.FontStudio.ViewModels
{
    public partial class WorkspaceViewModel
    {

        
        public ICommand ModeCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }


        private void TapMode(string? mode)
        {
            switch (mode)
            {
                case "Move":
                    Instance?.SwitchMode<MoveController>();
                    break;
                case "Pen":
                    Instance?.SwitchMode<PenController>();
                    break;
                case "PenJoint":
                    Instance?.SwitchMode<PenJointController>();
                    break;
                default:
                    Instance?.SwitchMode<ViewController>();
                    break;
            }
        }

        private void TapExit()
        {
            App.Current.Exit();
        }

    }
}
