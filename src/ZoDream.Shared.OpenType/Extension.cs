using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType
{
    public static class Extension
    {

        /// <summary>
        /// read float, 2.14 format
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static float ReadF2Dot14(this BinaryReader reader)
        {
            return ((float)reader.ReadInt16()) / (1 << 14); /* Format 2.14 */
        }


        internal static GlyphBound ReadBounds(this BinaryReader reader)
        {
            return new GlyphBound(
                reader.ReadInt16(),//xmin
                reader.ReadInt16(), //ymin
                reader.ReadInt16(), //xmax
                reader.ReadInt16());//ymax
        }

        internal static RecordEntry[] ReadRecord(this EndianReader reader, int count)
        {
            return reader.ReadArray(count, () => {
                var tag = reader.ReadString(4);
                return new RecordEntry(tag == "\0\0\0\0" ? string.Empty : tag, reader.ReadUInt16());
            });
        }
        internal static ushort[] ReadUInt16Array(this EndianReader reader, int count)
        {
            return reader.ReadArray(count, reader.ReadUInt16);
        }
        internal static int ReadUInt24(this BinaryReader reader)
        {
            byte highByte = reader.ReadByte();
            return (highByte << 16) | reader.ReadUInt16();
        }
        /// <summary>
        /// 16.16 float format
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static float ReadFixed(this BinaryReader reader)
        {
            //16.16 format
            return (float)reader.ReadUInt32() / (1 << 16);
        }
        /// <summary>
        /// 指定长度的字节转成 int
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static int ReadInt32(this BinaryReader reader, int count)
        {
            return count switch
            {
                1 => reader.ReadByte(),
                2 => (reader.ReadByte() << 8) | (reader.ReadByte() << 0),
                3 => (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | (reader.ReadByte() << 0),
                4 => (reader.ReadByte() << 24) | (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | (reader.ReadByte() << 0),
                _ => 0,
            };
        }
    }
}
