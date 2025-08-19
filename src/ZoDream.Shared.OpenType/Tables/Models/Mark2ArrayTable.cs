namespace ZoDream.Shared.OpenType.Tables
{
    public class Mark2ArrayTable
    {
        

        internal readonly ushort _classCount;
        internal readonly AnchorPoint[] _anchorPoints;

        public Mark2ArrayTable(ushort classCount, AnchorPoint[] anchorPoints)
        {
            _classCount = classCount;
            _anchorPoints = anchorPoints;
        }
        public AnchorPoint GetAnchorPoint(int index, int markClassId)
        {
            return _anchorPoints[index * _classCount + markClassId];
        }

       
    }
}