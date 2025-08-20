namespace ZoDream.Shared.OpenType.Tables
{
    public class TupleVariationHeader
    {
        public ushort VariableDataSize;

        public int Flags;
        public ushort IndexToSharedTupleRecArray;

        public TupleRecord PeakTuple;
        public TupleRecord IntermediateStartTuple;
        public TupleRecord IntermediateEndTuple;

        public ushort[] PrivatePoints { get; internal set; }
        public short[] PackedDeltasXY { get; internal set; }
    }
}
