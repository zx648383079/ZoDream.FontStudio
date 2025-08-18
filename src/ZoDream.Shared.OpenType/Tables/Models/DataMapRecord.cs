namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct DataMapRecord
    {
        public readonly string Tag;
        public readonly uint DataOffset;
        public readonly uint DataLength;
        public DataMapRecord(string tag, uint dataOffset, uint dataLength)
        {
            Tag = tag;
            DataOffset = dataOffset;
            DataLength = dataLength;
        }
    }
}
