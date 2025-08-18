namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct AxisValueRecord
    {
        public readonly ushort axisIndex;
        public readonly float value;
        public AxisValueRecord(ushort axisIndex, float value)
        {
            this.axisIndex = axisIndex;
            this.value = value;
        }
    }
}
