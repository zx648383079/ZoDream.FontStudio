using System;

namespace ZoDream.Shared.ImageEditor
{
    [Flags]
    public enum PointerState : byte
    {
        Released = 0b1,
        Pressed = 0b10,
        Moved = 0b100,
        ReleasedMoved = 0b101,
        PressedMoved = 0b110,
    }
}
