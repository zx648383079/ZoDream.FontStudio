using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphVariationsTable : ITypefaceTable
    {
        public const string TableName = "gvar";

        public string Name => TableName;

        public ushort axisCount;
        public TupleRecord[] _sharedTuples;

        public GlyphVariableData[] _glyphVarDataArr;
    }
}
