namespace ZoDream.Shared.OpenType.Tables
{
    public struct KerningPair
    {
        /// <summary>
        /// left glyph index
        /// </summary>
        public readonly ushort left;
        /// <summary>
        /// right glyph index
        /// </summary>
        public readonly ushort right;
        /// <summary>
        /// n FUnits. If this value is greater than zero, the characters will be moved apart. If this value is less than zero, the character will be moved closer together.
        /// </summary>
        public readonly short value;
        public KerningPair(ushort left, ushort right, short value)
        {
            this.left = left;
            this.right = right;
            this.value = value;
        }
    }
}
