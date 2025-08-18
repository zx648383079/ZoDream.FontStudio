namespace ZoDream.Shared.OpenType.Tables
{
    public struct Ratio(byte charset, byte xRatio, byte yStartRatio, byte yEndRatio)
    {
        public readonly byte Charset => charset;
        public readonly byte XRatio => xRatio;
        public readonly byte YStartRatio => yStartRatio;
        public readonly byte YEndRatio => yEndRatio;
    }
}
