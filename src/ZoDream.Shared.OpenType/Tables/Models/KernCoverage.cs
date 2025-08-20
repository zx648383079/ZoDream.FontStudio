namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct KernCoverage
    {
        public readonly ushort Coverage;
        public readonly bool Horizontal;
        public readonly bool HasMinimum;
        public readonly bool CrossStream;
        public readonly bool Override;
        public readonly byte Format;
        public KernCoverage(ushort coverage)
        {
            this.Coverage = coverage;
            //bit 0,len 1, 1 if table has horizontal data, 0 if vertical.
            Horizontal = (coverage & 0x1) == 1;
            //bit 1,len 1, If this bit is set to 1, the table has minimum values. If set to 0, the table has kerning values.
            HasMinimum = ((coverage >> 1) & 0x1) == 1;
            //bit 2,len 1, If set to 1, kerning is perpendicular to the flow of the text.
            CrossStream = ((coverage >> 2) & 0x1) == 1;
            //bit 3,len 1, If this bit is set to 1 the value in this table should replace the value currently being accumulated.
            Override = ((coverage >> 3) & 0x1) == 1;
            //bit 4-7 => 	Reserved. This should be set to zero.
            Format = (byte)((coverage >> 8) & 0xff);
        }
    }
}
