using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class PostTable : ITypefaceTable
    {
        public const string TableName = "post";

        public string Name => TableName;

        public float Version { get; set; }
        public uint ItalicAngle { get; set; }
        public short UnderlinePosition { get; set; }
        public short UnderlineThickness { get; set; }
    }
}
