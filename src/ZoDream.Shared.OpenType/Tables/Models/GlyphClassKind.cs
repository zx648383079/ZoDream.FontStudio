namespace ZoDream.Shared.OpenType.Tables
{
    public enum GlyphClassKind : byte
    {
        Zero = 0,//class0, classZero
        /// <summary>
        /// Base glyph (single character, spacing glyph)
        /// </summary>
        Base,
        /// <summary>
        /// Ligature glyph (multiple character, spacing glyph)
        /// </summary>
        Ligature,
        /// <summary>
        /// Mark glyph (non-spacing combining glyph)
        /// </summary>
        Mark,
        /// <summary>
        /// Component glyph (part of single character, spacing glyph)
        /// </summary>
        Component
    }
}
