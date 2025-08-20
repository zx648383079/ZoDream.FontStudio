namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt4 : AxisValueTableBase
    {
        public override int Format => 4;


        public AxisValueRecord[] ValueRecords;
        public ushort Flags;
        public ushort ValueNameId;
    }
}
