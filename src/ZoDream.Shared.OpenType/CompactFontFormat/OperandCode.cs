namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public readonly struct OperandCode
    {
        public readonly OperandKind Kind;
        public readonly double RealNumValue;
        public OperandCode(double number, OperandKind kind)
        {
            Kind = kind;
            RealNumValue = number;
        }
    }
}
