namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct IndexSubHeader
    {
        public readonly ushort indexFormat;
        public readonly ushort imageFormat;
        public readonly uint imageDataOffset;

        public IndexSubHeader(ushort indexFormat,
            ushort imageFormat, uint imageDataOffset)
        {
            this.indexFormat = indexFormat;
            this.imageFormat = imageFormat;
            this.imageDataOffset = imageDataOffset;
        }
    }
}
