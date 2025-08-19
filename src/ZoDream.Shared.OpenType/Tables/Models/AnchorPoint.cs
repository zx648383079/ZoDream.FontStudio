namespace ZoDream.Shared.OpenType.Tables
{
    public class AnchorPoint
    {
        public ushort format;
        public short xcoord;
        public short ycoord;
        /// <summary>
        /// an index to a glyph contour point (AnchorPoint)
        /// </summary>
        public ushort refGlyphContourPoint;
        public ushort xdeviceTableOffset;
        public ushort ydeviceTableOffset;
    }
}
