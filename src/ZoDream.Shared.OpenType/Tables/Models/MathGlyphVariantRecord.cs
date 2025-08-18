namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct MathGlyphVariantRecord
    {
        public readonly ushort VariantGlyph;
        public readonly ushort AdvanceMeasurement;
        public MathGlyphVariantRecord(ushort variantGlyph, ushort advanceMeasurement)
        {
            VariantGlyph = variantGlyph;
            AdvanceMeasurement = advanceMeasurement;
        }
    }
}
