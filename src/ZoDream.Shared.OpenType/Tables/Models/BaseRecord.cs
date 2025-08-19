namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct BaseRecord
    {
        public readonly AnchorPoint[] anchors;

        public BaseRecord(AnchorPoint[] anchors)
        {
            this.anchors = anchors;
        }
    }
}