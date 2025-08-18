namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct KernCoverage
    {
        public readonly ushort coverage;
        public readonly bool horizontal;
        public readonly bool hasMinimum;
        public readonly bool crossStream;
        public readonly bool _override;
        public readonly byte format;
        public KernCoverage(ushort coverage)
        {
            this.coverage = coverage;
            //bit 0,len 1, 1 if table has horizontal data, 0 if vertical.
            horizontal = (coverage & 0x1) == 1;
            //bit 1,len 1, If this bit is set to 1, the table has minimum values. If set to 0, the table has kerning values.
            hasMinimum = ((coverage >> 1) & 0x1) == 1;
            //bit 2,len 1, If set to 1, kerning is perpendicular to the flow of the text.
            crossStream = ((coverage >> 2) & 0x1) == 1;
            //bit 3,len 1, If this bit is set to 1 the value in this table should replace the value currently being accumulated.
            _override = ((coverage >> 3) & 0x1) == 1;
            //bit 4-7 => 	Reserved. This should be set to zero.
            format = (byte)((coverage >> 8) & 0xff);
        }
    }
}
