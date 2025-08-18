namespace ZoDream.Shared.OpenType.Tables
{
    public class BitmapSizeTable
    {
        public uint IndexSubTableArrayOffset;
        public uint IndexTablesSize;
        public uint NumberOfIndexSubTables;
        public uint ColorRef;

        public SbitLineMetrics Hori;
        public SbitLineMetrics Vert;

        public ushort StartGlyphIndex;
        public ushort EndGlyphIndex;

        public byte PpemX;
        public byte PpemY;
        public byte BitDepth;

        //bitDepth
        //Value   Description
        //1	      black/white
        //2	      4 levels of gray
        //4	      16 levels of gray
        //8	      256 levels of gray

        public sbyte Flags;

        //-----
        //reconstructed 
        public IndexSubTableBase[] IndexSubTables;
    }
}
