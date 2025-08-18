namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct ValueRecord
    {
        public readonly string Tag;
        public readonly ushort DeltaSetOuterIndex;
        public readonly ushort DeltaSetInnerIndex;
        public ValueRecord(string tag, ushort deltaSetOuterIndex, ushort deltaSetInnerIndex)
        {
            Tag = tag;
            DeltaSetOuterIndex = deltaSetOuterIndex;
            DeltaSetInnerIndex = deltaSetInnerIndex;
        }
    }
}
