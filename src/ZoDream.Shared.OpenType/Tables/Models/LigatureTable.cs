namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct LigatureTable
    {
        public readonly ushort GlyphId;
        /// <summary>
        /// ligature component start with second ordered glyph
        /// </summary>
        public readonly ushort[] ComponentGlyphs;

        public LigatureTable(ushort glyphId, ushort[] componentGlyphs)
        {
            GlyphId = glyphId;
            ComponentGlyphs = componentGlyphs;
        }
    }
}