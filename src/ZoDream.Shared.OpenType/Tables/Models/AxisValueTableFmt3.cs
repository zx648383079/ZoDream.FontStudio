namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt3 : AxisValueTableBase
    {
        public override int Format => 3;


        public ushort axisIndex;
        public ushort flags;
        public ushort valueNameId;
        public float value;
        public float linkedValue;
    }
}
