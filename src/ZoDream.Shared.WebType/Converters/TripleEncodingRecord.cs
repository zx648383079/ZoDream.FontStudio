namespace ZoDream.Shared.WebType.Converters
{
    internal readonly struct TripleEncodingRecord
    {
        public readonly byte ByteCount;
        public readonly byte XBits;
        public readonly byte YBits;
        public readonly ushort DeltaX;
        public readonly ushort DeltaY;
        public readonly sbyte Xsign;
        public readonly sbyte Ysign;

        public TripleEncodingRecord(
            byte byteCount,
            byte xbits, byte ybits,
            ushort deltaX, ushort deltaY,
            sbyte xsign, sbyte ysign)
        {
            ByteCount = byteCount;
            XBits = xbits;
            YBits = ybits;
            DeltaX = deltaX;
            DeltaY = deltaY;
            Xsign = xsign;
            Ysign = ysign;
        }

        /// <summary>
        /// translate X
        /// </summary>
        /// <param name="orgX"></param>
        /// <returns></returns>
        public int Tx(int orgX) => (orgX + DeltaX) * Xsign;

        /// <summary>
        /// translate Y
        /// </summary>
        /// <param name="orgY"></param>
        /// <returns></returns>
        public int Ty(int orgY) => (orgY + DeltaY) * Ysign;
    }
}