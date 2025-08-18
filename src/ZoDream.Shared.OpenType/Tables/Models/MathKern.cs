namespace ZoDream.Shared.OpenType.Tables
{
    public class MathKern
    {
        //reference =>see  MathKernTable
        public ushort HeightCount;
        public MathValueRecord[] CorrectionHeights;
        public MathValueRecord[] KernValues;

        public MathKern(ushort heightCount, MathValueRecord[] correctionHeights, MathValueRecord[] kernValues)
        {
            HeightCount = heightCount;
            CorrectionHeights = correctionHeights;
            KernValues = kernValues;
        }
    }
}
