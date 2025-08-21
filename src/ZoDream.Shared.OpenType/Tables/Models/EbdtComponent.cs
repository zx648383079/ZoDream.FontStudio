namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct EbdtComponent(ushort glyphID, sbyte xOffset, sbyte yOffset)
    {
        public ushort GlyphID => glyphID;
        public sbyte XOffset => xOffset;
        public sbyte YOffset => yOffset;
    }
}
