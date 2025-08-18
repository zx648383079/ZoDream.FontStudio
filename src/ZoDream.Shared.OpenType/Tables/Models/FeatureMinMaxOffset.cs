namespace ZoDream.Shared.OpenType.Tables
{
    readonly struct FeatureMinMaxOffset
    {
        public readonly string featureTableTag;
        public readonly ushort minCoord;
        public readonly ushort maxCoord;
        public FeatureMinMaxOffset(string featureTableTag, ushort minCoord, ushort maxCoord)
        {
            this.featureTableTag = featureTableTag;
            this.minCoord = minCoord;
            this.maxCoord = maxCoord;
        }
    }
}
