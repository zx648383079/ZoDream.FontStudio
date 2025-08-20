namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisValueTableFmt1 : AxisValueTableBase
    {
        public override int Format => 1;
        public ushort Index;
        public ushort Flags;
        public ushort ValueNameId;
        public float Value;
    }
}
