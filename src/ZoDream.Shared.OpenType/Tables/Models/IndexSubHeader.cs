namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct IndexSubHeader
    {
        public readonly ushort IndexFormat;
        public readonly ushort ImageFormat;
        public readonly uint ImageDataOffset;

        public IndexSubHeader(ushort indexFormat,
            ushort imageFormat, uint imageDataOffset)
        {
            IndexFormat = indexFormat;
            ImageFormat = imageFormat;
            ImageDataOffset = imageDataOffset;
        }
    }
}
