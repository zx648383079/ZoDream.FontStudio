using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class AxisVariationsTable : ITypefaceTable
    {
        public const string TableName = "avar";

        public string Name => TableName;

        public AxisValuePair[][] AxisSegmentMaps;
    }

    public struct AxisValuePair(float fromCoordinate, float toCoordinate)
    {
        public readonly float FromCoordinate => fromCoordinate;
        public readonly float ToCoordinate => toCoordinate;
    }
}
