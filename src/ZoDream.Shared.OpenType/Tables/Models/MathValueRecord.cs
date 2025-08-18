namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct MathValueRecord
    {
        public readonly short Value;
        public readonly ushort DeviceTable;
        public MathValueRecord(short value, ushort deviceTable)
        {
            Value = value;
            DeviceTable = deviceTable;
        }
    }
}
