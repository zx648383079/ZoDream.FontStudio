namespace ZoDream.Shared.OpenType.Tables
{
    internal struct NameRecord
    {
        public ushort PlatformID;
        public ushort EncodingID;
        public ushort LanguageID;
        public ushort NameID;
        public ushort StringLength;
        public ushort StringOffset;
    }
}
