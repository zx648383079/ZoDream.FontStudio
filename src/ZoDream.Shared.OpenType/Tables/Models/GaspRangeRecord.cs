namespace ZoDream.Shared.OpenType.Tables
{
    public struct GaspRangeRecord(ushort rangeMaxPPEM, GaspRangeBehavior rangeGaspBehavior)
    {
        public readonly ushort rangeMaxPPEM => rangeMaxPPEM;
        public readonly GaspRangeBehavior rangeGaspBehavior => rangeGaspBehavior;
    }
}
