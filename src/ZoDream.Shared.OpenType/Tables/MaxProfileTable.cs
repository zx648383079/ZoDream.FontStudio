using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class MaxProfileTable : ITypefaceTable
    {
        public const string TableName = "maxp";

        public string Name => TableName;

        public uint Version { get; set; }
        public ushort GlyphCount { get; set; }
        public ushort MaxPointsPerGlyph { get; set; }
        public ushort MaxContoursPerGlyph { get; set; }
        public ushort MaxPointsPerCompositeGlyph { get; set; }
        public ushort MaxContoursPerCompositeGlyph { get; set; }
        public ushort MaxZones { get; set; }
        public ushort MaxTwilightPoints { get; set; }
        public ushort MaxStorage { get; set; }
        public ushort MaxFunctionDefs { get; set; }
        public ushort MaxInstructionDefs { get; set; }
        public ushort MaxStackElements { get; set; }
        public ushort MaxSizeOfInstructions { get; set; }
        public ushort MaxComponentElements { get; set; }
        public ushort MaxComponentDepth { get; set; }
    }
}
