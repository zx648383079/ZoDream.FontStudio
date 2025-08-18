using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalMetricsVariationsTable : ITypefaceTable
    {
   

        public const string TableName = "HVAR";

        public string Name => TableName;


        internal ItemVariationStoreTable _itemVartionStore;
        internal DeltaSetIndexMap[] _advanceWidthMapping;
        internal DeltaSetIndexMap[] _lsbMapping;
        internal DeltaSetIndexMap[] _rsbMapping;
    }
}
