using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType
{
    public class TTFReader(EndianReader reader) : ITypefaceReader
    {
        public TTFReader(Stream input) : this(new EndianReader(input, EndianType.BigEndian, false))
        {

        }

        public ITypefaceCollection Read()
        {
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            var res = new TypefaceCollection
            {
                ReadTypeface()
            };
            return res;
        }

        public ITypeface ReadTypeface()
        {
            var res = new Typeface();
            var tableCount = reader.ReadUInt16();
            var searchRange = reader.ReadUInt16();
            var entrySelector = reader.ReadUInt16();
            var rangeShift = reader.ReadUInt16();
            var entries = new TypefaceTableEntry[tableCount];
            for (int i = 0; i < tableCount; i++)
            {
                entries[i] = ReadTableEntry();
            }
            foreach (var item in entries)
            {
                reader.Position = item.Offset;
                // 解 table
            }
            return res;
        }

        private TypefaceTableEntry ReadTableEntry()
        {
            return new TypefaceTableEntry()
            {
                Name = reader.ReadString(4),
                CheckSum = reader.ReadUInt32(),
                Offset = reader.ReadUInt32(),
                Length = reader.ReadUInt32(),
            };
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
