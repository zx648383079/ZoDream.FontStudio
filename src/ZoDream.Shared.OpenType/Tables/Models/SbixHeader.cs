using System.IO;

namespace ZoDream.Shared.OpenType.Tables
{

    public class SbixStrike
    {
        public ushort Ppem { get; set; }
        public ushort Ppi { get; set; }

        public SbixGlyphData[] GlyphItems { get; internal set; }
    }

    public class SbixGlyphData
    {
        public ushort OriginOffsetX { get; set; }
        public ushort OriginOffsetY { get; set; }
        /// <summary>
        /// 'jpg '、 'png '、'tiff'、'dupe'
        /// </summary>
        public string GraphicType { get; set; }

        public Stream Buffer { get; set; }
        // 指向其他 GlyphData 的索引
        public ushort GlyphId { get; set; }
    }
}
