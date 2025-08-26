using ZoDream.Shared.Font;

namespace ZoDream.Shared.WebType
{
    public class WOFFTableEntry : ITypefaceTableEntry
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public long Length { get; set; }

        public uint CompressedOffset;
        public uint CompressedLength;
        public uint OriginalChecksum;
        /// <summary>
        /// woff2 指示是否经过数据预处理
        /// </summary>
        public byte PreprocessingTransformation { get; set; }
        /// <summary>
        /// woff2 未转换之前的长度
        /// </summary>
        public long OriginalLength { get; set; }


    }
}
