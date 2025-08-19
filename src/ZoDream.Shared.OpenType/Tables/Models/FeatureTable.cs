namespace ZoDream.Shared.OpenType.Tables
{
    public class FeatureTable
    {
        public ushort[] LookupListIndices { get; set; }
        public string Tag { get; set; }
    }

    public class FeatureList
    {
        public FeatureTable[] FeatureTables;
    }
}
