namespace ZoDream.Shared.Font
{
    public class TypefaceTableEntry : ITypefaceTableEntry
    {
        public string Name { get; set; }
        public uint Offset { get; set; }
        public uint CheckSum { get; set; }
        public uint Length { get; set; }
    }
}
