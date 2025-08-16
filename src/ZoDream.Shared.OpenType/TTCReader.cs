using System.Diagnostics;
using System.IO;
using System.Linq;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType
{
    public class TTCReader(EndianReader reader) : ITypefaceReader
    {
        public static byte[] Signature = "ttcf"u8.ToArray();

        public TTCReader(Stream input): this(new EndianReader(input, EndianType.BigEndian))
        {
            
        }



        public ITypefaceCollection Read()
        {
            var buffer = reader.ReadBytes(Signature.Length);
            Debug.Assert(buffer.SequenceEqual(Signature));
            var header = ReadHeader();
            var res = new TypefaceCollection();
            var ttf = new TTFReader(reader);
            foreach (var offset in header.OffsetTables)
            {
                reader.Position = offset;
                res.Add(ttf.ReadTypeface());
            }
            return res;
        }

        private TTCFileHeader ReadHeader()
        {
            var header = new TTCFileHeader
            {
                MajorVersion = reader.ReadUInt16(),
                MinorVersion = reader.ReadUInt16(),
                OffsetTables = reader.ReadArray(reader.ReadInt32)
            };

            if (header.MajorVersion == 2)
            {
                header.DsigTag = reader.ReadUInt32();
                header.DsigLength = reader.ReadUInt32();
                header.DsigOffset = reader.ReadUInt32();

                if (header.DsigTag == 0x44534947)
                {
                    //Tag indicating that a DSIG table exists
                    //TODO: goto DSIG add read signature
                }
            }
            return header;
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
