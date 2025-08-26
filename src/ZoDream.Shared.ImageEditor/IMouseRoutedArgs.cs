using SkiaSharp;

namespace ZoDream.Shared.ImageEditor
{
    public interface IMouseRoutedArgs
    {
        public SKPoint Position { get; }

        public PointerState State { get; }
        public bool IsLeftButtonPressed { get; }
        public bool IsRightButtonPressed { get; }
        public bool IsControlPressed { get; }
        public bool IsShiftPressed { get; }
    }

    public interface IKeyboardRoutedArgs
    {
        public bool IsControl { get; }
        public bool IsShift { get; }
    }
}
