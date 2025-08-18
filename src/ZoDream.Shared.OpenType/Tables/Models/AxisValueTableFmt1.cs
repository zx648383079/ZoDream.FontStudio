namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt1 : AxisValueTableBase
    {
        public override int Format => 1;
        public ushort axisIndex;
        public ushort flags;
        public ushort valueNameId;
        public float value;
    }
}
