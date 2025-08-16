namespace ZoDream.Shared.WebType
{
    public class WOFFFileHeader
    {
        public uint Flavor;
        public uint Length;
        public uint NumTables;
        public ushort Reserved;
        public uint TotalSfntSize;
        /// <summary>
        /// woff2
        /// </summary>
        public uint TotalCompressedSize;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint MetaOffset;
        public uint MetaLength;
        public uint MetaOriginalLength;
        public uint PrivOffset;
        public uint PrivLength;
    }
}
