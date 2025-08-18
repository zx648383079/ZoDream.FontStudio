namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct BaseValues
    {
        public readonly ushort defaultBaseLineIndex;
        public readonly BaseCoord[] baseCoords;

        public BaseValues(ushort defaultBaseLineIndex, BaseCoord[] baseCoords)
        {
            this.defaultBaseLineIndex = defaultBaseLineIndex;
            this.baseCoords = baseCoords;
        }
    }
}
