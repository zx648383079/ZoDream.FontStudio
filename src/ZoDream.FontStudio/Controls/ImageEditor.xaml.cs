using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.FontStudio.Controls
{
    public sealed partial class ImageEditor : UserControl
    {
        public ImageEditor()
        {
            InitializeComponent();
            Loaded += ImageEditor_Loaded;
        }

        public SKXamlCanvas CanvasTarget => PART_Canvas;

        private readonly double _dpiScale = App.ViewModel.GetDpiScaleFactorFromWindow();
        private bool _booted = false;

        private void ImageEditor_Loaded(object sender, RoutedEventArgs e)
        {
            _booted = true;
            Resize(new SKSize((float)(ActualWidth * _dpiScale), (float)(ActualHeight * _dpiScale)));
        }

        private void PART_Canvas_PaintSurface(object sender, SkiaSharp.Views.Windows.SKPaintSurfaceEventArgs e)
        {
            if (!_booted)
            {
                ImageEditor_Loaded(this, null);
            }
            Paint(e.Surface.Canvas, e.Info);
        }


        private void PART_Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var target = (UIElement)sender;
            var point = e.GetCurrentPoint(target);
            OnPointerPressed(new ImageMouseRoutedArgs(
                new((float)(point.Position.X * _dpiScale), (float)(point.Position.Y * _dpiScale)),
                point,
                e.KeyModifiers
                ));
        }

        private void PART_Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var target = (UIElement)sender;
            var point = e.GetCurrentPoint(target);
            OnPointerMoved(new ImageMouseRoutedArgs(
                new((float)(point.Position.X * _dpiScale), (float)(point.Position.Y * _dpiScale)),
                point,
                e.KeyModifiers
                ));
        }

        private void PART_Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var target = (UIElement)sender;
            var point = e.GetCurrentPoint(target);
            OnPointerReleased(new ImageMouseRoutedArgs(
                new((float)(point.Position.X * _dpiScale), (float)(point.Position.Y * _dpiScale)),
                point,
                e.KeyModifiers
                ));
        }

        private void PART_Canvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            OnPointerPressed(
                new ImageMouseRoutedArgs(
                new ((float)(e.Position.X * _dpiScale), (float)(e.Position.Y * _dpiScale))));
        }

        private void PART_Canvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            OnPointerPressed(new ImageMouseRoutedArgs(
                new((float)(e.Position.X * _dpiScale), (float)(e.Position.Y * _dpiScale))));
        }

        private void PART_Canvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            OnPointerReleased(new ImageMouseRoutedArgs(new((float)(e.Position.X * _dpiScale), (float)(e.Position.Y * _dpiScale))));
        }

        private void PART_Canvas_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            OnKeyPressed(new ImageKeyboardRoutedArgs(e));
        }

        private void PART_Canvas_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            OnKeyReleased(new ImageKeyboardRoutedArgs(e));
        }

        private void ResizeWithControl(SKSize size)
        {
            CanvasTarget.Width = size.Width / _dpiScale;
            CanvasTarget.Height = size.Height / _dpiScale;
        }


    }
}
