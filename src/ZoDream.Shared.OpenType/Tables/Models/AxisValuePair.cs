namespace ZoDream.Shared.OpenType.Tables
{
    public struct AxisValuePair(float fromCoordinate, float toCoordinate)
    {
        public readonly float FromCoordinate => fromCoordinate;
        public readonly float ToCoordinate => toCoordinate;
    }
}
