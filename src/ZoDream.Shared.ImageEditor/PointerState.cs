using System;

namespace ZoDream.Shared.ImageEditor
{
    [Flags]
    public enum PointerState : byte
    {
        Released = 0b1,
        Pressed = 0b10,
        Moved = 0b100,
        /// <summary>
        /// 指定 Pressed 到 Released 中没有移动 
        /// </summary>
        NotMoved = 0b1000,
        ReleasedMoved = 0b101,
        PressedMoved = 0b110,
        NotMovedReleased = 0b1001,
    }
}
