namespace ZoDream.Shared.OpenType
{
    public class TTCFileHeader
    {
        public ushort MajorVersion;
        public ushort MinorVersion;
        public int[] OffsetTables;
        //
        //if version 2
        public uint DsigTag;
        public uint DsigLength;
        public uint DsigOffset;
    }
}
