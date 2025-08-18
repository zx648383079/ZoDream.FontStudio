using System;

namespace ZoDream.Shared.OpenType.Tables
{
    [Flags]
    public enum GaspRangeBehavior : ushort
    {
        Neither = 0,
        GASP_DOGRAY = 0x0002,
        GASP_GRIDFIT = 0x0001,
        GASP_DOGRAY_GASP_GRIDFIT = 0x0003,
        GASP_SYMMETRIC_GRIDFIT = 0x0004,
        GASP_SYMMETRIC_SMOOTHING = 0x0008,
        GASP_SYMMETRIC_SMOOTHING_GASP_SYMMETRIC_GRIDFIT = 0x000C
    }
}
