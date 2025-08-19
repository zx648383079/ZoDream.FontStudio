namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class Operator
    {
        readonly byte _b0;
        readonly byte _b1;
        readonly OperatorOperandKind _operatorOperandKind;

        public string Name { get; }

        //b0 the first byte of a two byte value
        //b1 the second byte of a two byte value
        internal Operator(string name, byte b0, byte b1, OperatorOperandKind operatorOperandKind)
        {
            _b0 = b0;
            _b1 = b1;
            Name = name;
            _operatorOperandKind = operatorOperandKind;
        }

 
    }
}
