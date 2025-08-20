using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class Pcl5Table : ITypefaceTable
    {
        public const string TableName = "PCLT";

        public string Name => TableName;

        public uint FontNumber { get; internal set; }
        public ushort Pitch { get; internal set; }
        public ushort XHeight { get; internal set; }
        public ushort Style { get; internal set; }
        public ushort TypeFamily { get; internal set; }
        public ushort CapHeight { get; internal set; }
        public ushort SymbolSet { get; internal set; }
        public byte[] Typeface { get; internal set; }
        public byte[] CharacterComplement { get; internal set; }
        public byte[] FileName { get; internal set; }
        public sbyte StrokeWeight { get; internal set; }
        public sbyte WidthType { get; internal set; }
        public byte SerifStyle { get; internal set; }
    }
}
