using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.FontStudio.Controls
{
    public sealed partial class PathJointController : Control
    {
        internal const string NormalState = "Normal";
        internal const string PointerOverState = "PointerOver";
        internal const string PressedState = "Pressed";
        internal const string DisabledState = "Disabled";
        public PathJointController()
        {
            DefaultStyleKey = typeof(PathJointController);
        }

        protected override void OnApplyTemplate()
        {
            PointerEntered -= Control_PointerEntered;
            PointerExited -= Control_PointerExited;
            PointerCaptureLost -= Control_PointerCaptureLost;
            PointerCanceled -= Control_PointerCanceled;

            PointerEntered += Control_PointerEntered;
            PointerExited += Control_PointerExited;
            PointerCaptureLost += Control_PointerCaptureLost;
            PointerCanceled += Control_PointerCanceled;
        }

        public void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, PointerOverState, true);
        }

        public void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, NormalState, true);
        }

        private void Control_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            base.OnPointerCaptureLost(e);
            VisualStateManager.GoToState(this, NormalState, true);
        }

        private void Control_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            base.OnPointerCanceled(e);
            VisualStateManager.GoToState(this, NormalState, true);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, PressedState, true);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            VisualStateManager.GoToState(this, NormalState, true);
        }
    }
}
