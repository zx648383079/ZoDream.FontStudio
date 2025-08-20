namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt2 : AxisValueTableBase
    {
        public override int Format => 2;

        public ushort Index;
        public ushort Flags;
        public ushort ValueNameId;
        public float NominalValue;
        public float RangeMinValue;
        public float RangeMaxValue;
    }
}
