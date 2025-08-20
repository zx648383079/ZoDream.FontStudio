namespace ZoDream.Shared.OpenType.Tables
{
    public struct KernPair
    {
        /// <summary>
        /// left glyph index
        /// </summary>
        public readonly ushort Left;
        /// <summary>
        /// right glyph index
        /// </summary>
        public readonly ushort Right;
        /// <summary>
        /// n FUnits. If this value is greater than zero, the characters will be moved apart. If this value is less than zero, the character will be moved closer together.
        /// </summary>
        public readonly short Value;
        public KernPair(ushort left, ushort right, short value)
        {
            Left = left;
            Right = right;
            Value = value;
        }
    }
}
