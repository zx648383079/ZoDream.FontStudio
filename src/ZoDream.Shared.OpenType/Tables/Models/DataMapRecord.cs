namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct DataMapRecord
    {
        public readonly string Tag;
        public readonly uint Offset;
        public readonly uint Length;
        public DataMapRecord(string tag, uint dataOffset, uint dataLength)
        {
            Tag = tag;
            Offset = dataOffset;
            Length = dataLength;
        }
    }
}
