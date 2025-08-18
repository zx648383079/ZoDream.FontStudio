namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt4 : AxisValueTableBase
    {
        public override int Format => 4;


        public AxisValueRecord[] _axisValueRecords;
        public ushort flags;
        public ushort valueNameId;
    }
}
