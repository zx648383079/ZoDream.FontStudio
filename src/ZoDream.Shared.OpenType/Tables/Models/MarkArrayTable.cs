namespace ZoDream.Shared.OpenType.Tables
{
    public class MarkArrayTable
    {
        internal MarkRecord[] _records;
        internal AnchorPoint[] _anchorPoints;
        public AnchorPoint GetAnchorPoint(int index)
        {
            return _anchorPoints[index];
        }
        public ushort GetMarkClass(int index)
        {
            return _records[index].markClass;
        }
    }
}