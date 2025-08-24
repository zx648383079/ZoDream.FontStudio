namespace ZoDream.Shared.ImageEditor
{
    public interface IMouseState
    {
        public void PointerPressed(IMouseRoutedArgs args);
        public void PointerMoved(IMouseRoutedArgs args);
        public void PointerReleased(IMouseRoutedArgs args);
    }

    public interface IKeyboardState
    {
        public void KeyPressed(IKeyboardRoutedArgs args);
        public void KeyReleased(IKeyboardRoutedArgs args);
    }
}
