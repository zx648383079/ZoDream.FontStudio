namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public readonly struct Type2Instruction
    {
        public readonly int Value;
        public readonly byte Op;

        internal bool IsLoadInt => (Operand)Op == Operand.LoadInt;
        public Type2Instruction(Operand op, int value)
        {
            this.Op = (byte)op;
            this.Value = value;
        }
        public Type2Instruction(byte op, int value)
        {
            this.Op = op;
            this.Value = value;
        }
        public Type2Instruction(Operand op)
        {
            this.Op = (byte)op;
            this.Value = 0;
        }
    }
}
