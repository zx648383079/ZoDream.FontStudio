namespace ZoDream.Shared.OpenType.Tables
{
    public class TupleVariationHeader
    {
        public ushort variableDataSize;

        public int flags;
        public ushort indexToSharedTupleRecArray;

        public TupleRecord peakTuple;
        public TupleRecord intermediateStartTuple;
        public TupleRecord intermediateEndTuple;

        public ushort[] PrivatePoints { get; internal set; }
        public short[] PackedDeltasXY { get; internal set; }
    }
}
