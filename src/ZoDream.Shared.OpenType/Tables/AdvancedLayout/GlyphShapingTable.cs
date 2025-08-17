using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class GlyphShapingTable : ITypefaceTable
    {
        public abstract string Name { get; }
    }
}
