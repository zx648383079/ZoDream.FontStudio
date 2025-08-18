namespace ZoDream.Shared.OpenType.Tables
{
    public struct AdvanceHeightAndTopSideBearing(ushort advanceHeight, short topSideBearing)
    {
        public readonly ushort AdvanceHeight => advanceHeight;
        public readonly short TopSideBearing => topSideBearing;
    }
}
