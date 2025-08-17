namespace ZoDream.Shared.Font
{
    public class TypefaceTableEntry : ITypefaceTableEntry
    {
        public string Name { get; set; }
        public long Offset { get; set; }
        public uint CheckSum { get; set; }
        public long Length { get; set; }
    }
}
