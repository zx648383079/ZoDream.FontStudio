namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct RecordEntry(string tag, ushort offset)
    {
        public string Tag => tag;
        public ushort Offset => offset;
    }
}
