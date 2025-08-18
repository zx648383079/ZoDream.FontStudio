
namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class CoverageTable
    {
    }

    public class CoverageFmt1 : CoverageTable
    {
        internal ushort[] OrderedGlyphIdList;
    }

    public class CoverageFmt2 : CoverageTable
    {
        internal ushort[] StartIndices;
        internal ushort[] EndIndices;
        internal ushort[] CoverageIndices;
    }
}
