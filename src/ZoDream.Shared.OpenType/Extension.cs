using System.IO;

namespace ZoDream.Shared.OpenType
{
    public static class Extension
    {

        /// <summary>
        /// read float, 2.14 format
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static float ReadF2Dot14(this BinaryReader reader)
        {
            return ((float)reader.ReadInt16()) / (1 << 14); /* Format 2.14 */
        }


        public static GlyphBound ReadBounds(this BinaryReader reader)
        {
            return new GlyphBound(
                reader.ReadInt16(),//xmin
                reader.ReadInt16(), //ymin
                reader.ReadInt16(), //xmax
                reader.ReadInt16());//ymax
        }


        public static int ReadUInt24(this BinaryReader reader)
        {
            byte highByte = reader.ReadByte();
            return (highByte << 16) | reader.ReadUInt16();
        }
        /// <summary>
        /// 16.16 float format
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static float ReadFixed(this BinaryReader reader)
        {
            //16.16 format
            return (float)reader.ReadUInt32() / (1 << 16);
        }
    }
}
