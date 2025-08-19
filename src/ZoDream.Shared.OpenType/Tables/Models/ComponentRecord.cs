namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct ComponentRecord
    {
        public readonly ushort[] offsets;
        public ComponentRecord(ushort[] offsets)
        {
            this.offsets = offsets;
        }
    }
}