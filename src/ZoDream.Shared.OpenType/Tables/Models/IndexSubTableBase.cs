namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class IndexSubTableBase
    {
        public IndexSubHeader header;

        public abstract int SubTypeNo { get; }
        public ushort firstGlyphIndex;
        public ushort lastGlyphIndex;
    }
}
