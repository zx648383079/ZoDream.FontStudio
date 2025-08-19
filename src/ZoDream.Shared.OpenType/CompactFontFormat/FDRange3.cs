namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public readonly struct FDRange3
    {
        public readonly ushort first;
        public readonly byte fd;
        public FDRange3(ushort first, byte fd)
        {
            this.first = first;
            this.fd = fd;
        }
    }
}
