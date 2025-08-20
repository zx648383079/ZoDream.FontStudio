namespace ZoDream.Shared.OpenType.Tables
{
    public class KernSubtableFormat2 : IKernSubtable
    {
        public ushort RowWidth { get; internal set; }
        public KernSubtableClassPair LeftOffset { get; internal set; }
        public KernSubtableClassPair RightOffset { get; internal set; }
    }

    public class KernSubtableClassPair
    {
        public ushort FirstGlyph { get; set; }
        public ushort[] Glyphs { get; set; }
    }
}
