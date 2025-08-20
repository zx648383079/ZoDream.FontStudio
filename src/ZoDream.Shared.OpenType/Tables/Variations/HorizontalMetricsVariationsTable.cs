using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalMetricsVariationsTable : ITypefaceTable
    {
   

        public const string TableName = "HVAR";

        public string Name => TableName;


        internal ItemVariationStoreTable ItemVartionStore;
        internal DeltaSetIndexMap[] AdvanceWidthMapping;
        internal DeltaSetIndexMap[] LsbMapping;
        internal DeltaSetIndexMap[] RsbMapping;
    }
}
