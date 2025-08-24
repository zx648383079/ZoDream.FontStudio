using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using SkiaSharp;
using System;
using Windows.System;
using ZoDream.Shared.ImageEditor;

namespace ZoDream.FontStudio.Controls
{
    public class ImageMouseRoutedArgs(SKPoint point, PointerPoint? source = null, VirtualKeyModifiers? modifiers = null) : IMouseRoutedArgs
    {
        public SKPoint Position => point;

        public bool IsLeftButtonPressed => source?.Properties?.IsLeftButtonPressed == true;

        public bool IsRightButtonPressed => source?.Properties?.IsRightButtonPressed == true;

        public bool IsControlPressed => modifiers?.HasFlag(VirtualKeyModifiers.Control) == true;

        public bool IsShiftPressed => modifiers?.HasFlag(VirtualKeyModifiers.Shift) == true;
    }

    public class ImageKeyboardRoutedArgs(KeyRoutedEventArgs args) : IKeyboardRoutedArgs
    {
        public bool IsControl => args.Key == VirtualKey.Control;

        public bool IsShift => args.Key == VirtualKey.Shift;
    }
}
