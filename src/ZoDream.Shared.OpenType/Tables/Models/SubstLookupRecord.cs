namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct SubstLookupRecord
    {
        public readonly ushort sequenceIndex;
        public readonly ushort lookupListIndex;
        public SubstLookupRecord(ushort seqIndex, ushort lookupListIndex)
        {
            this.sequenceIndex = seqIndex;
            this.lookupListIndex = lookupListIndex;
        }
    }
}