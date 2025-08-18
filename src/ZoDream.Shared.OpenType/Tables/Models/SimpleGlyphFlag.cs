using System;

namespace ZoDream.Shared.OpenType.Tables
{
    [Flags]
    public enum SimpleGlyphFlag : byte
    {
        OnCurve = 1,
        XByte = 1 << 1,
        YByte = 1 << 2,
        Repeat = 1 << 3,
        XSignOrSame = 1 << 4,
        YSignOrSame = 1 << 5
    }
}
