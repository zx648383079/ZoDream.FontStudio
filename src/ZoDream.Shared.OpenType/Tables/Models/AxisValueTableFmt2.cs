namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt2 : AxisValueTableBase
    {
        public override int Format => 2;

        public ushort axisIndex;
        public ushort flags;
        public ushort valueNameId;
        public float nominalValue;
        public float rangeMinValue;
        public float rangeMaxValue;
    }
}
