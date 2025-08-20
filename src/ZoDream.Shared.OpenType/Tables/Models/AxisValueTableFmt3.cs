namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt3 : AxisValueTableBase
    {
        public override int Format => 3;


        public ushort Index;
        public ushort Flags;
        public ushort ValueNameId;
        public float Value;
        public float LinkedValue;
    }
}
