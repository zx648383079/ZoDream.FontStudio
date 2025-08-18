namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct RegionAxisCoordinate
    {
        public readonly float startCoord;
        public readonly float peakCoord;
        public readonly float endCoord;

        public RegionAxisCoordinate(float startCoord, float peakCoord, float endCoord)
        {
            this.startCoord = startCoord;
            this.peakCoord = peakCoord;
            this.endCoord = endCoord;
        }
    }
}
