using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HeadTable : ITypefaceTable
    {
        public const string TableName = "head";

        public string Name => TableName;

        public short IndexToLocFormat { get; set; }

        public uint Version { get; set; }
        public uint FontRevision { get; set; }
        public uint CheckSumAdjustment { get; set; }
        public uint MagicNumber { get; set; }
        public ushort Flags { get; set; }
        public ushort UnitsPerEm { get; set; }
        public ulong Created { get; set; }
        public ulong Modified { get; set; }
        public GlyphBound Bounds { get; set; }
        public ushort MacStyle { get; set; }
        public ushort LowestRecPPEM { get; set; }
        public short FontDirectionHint { get; set; }

        public short GlyphDataFormat { get; set; }

        public bool WideGlyphLocations => IndexToLocFormat > 0;
    }
}
