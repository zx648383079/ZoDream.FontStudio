namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct PosLookupRecord
    {
        public readonly ushort seqIndex;
        public readonly ushort lookupListIndex;
        public PosLookupRecord(ushort seqIndex, ushort lookupListIndex)
        {
            this.seqIndex = seqIndex;
            this.lookupListIndex = lookupListIndex;
        }
    }
}