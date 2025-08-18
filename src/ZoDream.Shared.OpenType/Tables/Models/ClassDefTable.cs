namespace ZoDream.Shared.OpenType.Tables
{
    public class ClassDefTable
    {
        public int Format { get; internal set; }
        //----------------
        //format 1
        public ushort startGlyph;
        public ushort[] classValueArray;
        //---------------
        //format2
        public ClassRangeRecord[] records;
    }
}
