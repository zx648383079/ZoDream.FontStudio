namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct DeltaSetIndexMap
    {
        internal const int INNER_INDEX_BIT_COUNT_MASK = 0x000F;
        internal const int MAP_ENTRY_SIZE_MASK = 0x0030;
        internal const int MAP_ENTRY_SIZE_SHIFT = 4;

        public readonly ushort innerIndex;
        public readonly ushort outerIndex;

        public DeltaSetIndexMap(ushort innerIndex, ushort outerIndex)
        {
            this.innerIndex = innerIndex;
            this.outerIndex = outerIndex;
        }
    }
}
