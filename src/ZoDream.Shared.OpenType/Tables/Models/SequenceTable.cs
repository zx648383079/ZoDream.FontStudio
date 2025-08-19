namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct SequenceTable
    {
        public readonly ushort[] substituteGlyphs;
        public SequenceTable(ushort[] substituteGlyphs)
        {
            this.substituteGlyphs = substituteGlyphs;
        }
    }
}