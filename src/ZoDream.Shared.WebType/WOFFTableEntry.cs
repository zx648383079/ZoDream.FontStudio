using ZoDream.Shared.Font;

namespace ZoDream.Shared.WebType
{
    public class WOFFTableEntry : ITypefaceTableEntry
    {
        public string Name { get; set; }
        public uint Offset;
        public uint CompLength;
        public uint OrigLength;
        public uint OrigChecksum;

        public long ExpectedStartAt { get; set; }
        /// <summary>
        /// woff2
        /// </summary>
        public byte PreprocessingTransformation { get; set; }

        public uint TransformLength { get; set; }

        long ITypefaceTableEntry.Offset => throw new System.NotImplementedException();

        public long Length => throw new System.NotImplementedException();
    }
}
