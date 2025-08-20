using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class CVTVariationsTable : ITypefaceTable
    {
        public const string TableName = "cvar";

        public string Name => TableName;

        public TupleVariationHeader[] TupleVariationHeaders { get; internal set; }
    }
}
