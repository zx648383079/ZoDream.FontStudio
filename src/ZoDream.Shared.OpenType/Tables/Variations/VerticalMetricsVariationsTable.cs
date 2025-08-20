using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class VerticalMetricsVariationsTable : ITypefaceTable
    {
        public const string TableName = "VVAR";

        public string Name => TableName;

        public uint ItemVariationStoreOffset { get; internal set; }
        public uint AdvanceHeightMappingOffset { get; internal set; }
        public uint TsbMappingOffset { get; internal set; }
        public uint VOrgMappingOffset { get; internal set; }
        public uint BsbMappingOffset { get; internal set; }
    }
}
