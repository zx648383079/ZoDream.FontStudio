namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct FeatureMinMax
    {
        public readonly string featureTableTag;
        public readonly BaseCoord minCoord;
        public readonly BaseCoord maxCoord;
        public FeatureMinMax(string tag, BaseCoord minCoord, BaseCoord maxCoord)
        {
            featureTableTag = tag;
            this.minCoord = minCoord;
            this.maxCoord = maxCoord;
        }
    }
}
