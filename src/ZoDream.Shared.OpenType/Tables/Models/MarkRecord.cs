namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct MarkRecord
    {
        /// <summary>
        /// Class defined for this mark,. A mark class is identified by a specific integer, called a class value
        /// </summary>
        public readonly ushort markClass;
        /// <summary>
        /// Offset to Anchor table-from beginning of MarkArray table
        /// </summary>
        public readonly ushort offset;
        public MarkRecord(ushort markClass, ushort offset)
        {
            this.markClass = markClass;
            this.offset = offset;
        }
    }
}