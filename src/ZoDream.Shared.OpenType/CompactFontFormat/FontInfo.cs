namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class FontInfo
    {
        public string ROS_Register;
        public string ROS_Ordering;
        public string ROS_Supplement;

        public double CIDFontVersion;
        public int CIDFountCount;
        public int FDSelect;
        public int FDArray;

        public int fdSelectFormat;
        public FDRange3[] fdRanges;
    }
}
