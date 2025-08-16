using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.WebType
{
    public partial class WOFF2Reader(EndianReader reader) : ITypefaceReader
    {
        public static byte[] Signature = "wOF2"u8.ToArray();

        public WOFF2Reader(Stream input) : this(new EndianReader(input, EndianType.BigEndian))
        {

        }

        public ITypefaceCollection Read()
        {
            var buffer = reader.ReadBytes(Signature.Length);
            Debug.Assert(buffer.SequenceEqual(Signature));
            var res = new Typeface();
            var header = ReadHeader();
            var entries = ReadEntry(header).ToArray();
            new BrotliStream(new PartialStream(reader.BaseStream, header.TotalCompressedSize), 
                CompressionMode.Decompress);
            return new TypefaceCollection
            {
                res
            };
        }

        private IEnumerable<WOFFTableEntry> ReadEntry(WOFFFileHeader header)
        {
            var expectedStartAt = 0L;
            for (int i = 0; i < header.NumTables; i++)
            {
                var flags = reader.ReadByte();
                var knowTable = flags & 0x1F;
                var entry = new WOFFTableEntry()
                {
                    Name = (knowTable < 63) ? _knownTableTags[knowTable] : reader.ReadString(4),
                    PreprocessingTransformation = (byte)((flags >> 5) & 0x3),
                    ExpectedStartAt = expectedStartAt,
                    OrigLength = reader.Read7BitEncodedUInt(),
                };
                if (entry.PreprocessingTransformation == 0 && entry.Name is GlyphDataTable.TableName or GlyphLocationsTable.TableName)
                {
                    entry.TransformLength = reader.Read7BitEncodedUInt();
                    expectedStartAt += entry.TransformLength;
                }
                else
                {
                    expectedStartAt += entry.OrigLength;
                }
                yield return entry;
                
            }
        }

        private WOFFFileHeader ReadHeader()
        {
            var header = new WOFFFileHeader
            {
                Flavor = reader.ReadUInt32(),
                Length = reader.ReadUInt32(),
                NumTables = reader.ReadUInt16(),
                Reserved = reader.ReadUInt16(),
                TotalSfntSize = reader.ReadUInt32(),
                TotalCompressedSize = reader.ReadUInt32(),

                MajorVersion = reader.ReadUInt16(),
                MinorVersion = reader.ReadUInt16(),

                MetaOffset = reader.ReadUInt32(),
                MetaLength = reader.ReadUInt32(),
                MetaOriginalLength = reader.ReadUInt32(),

                PrivOffset = reader.ReadUInt32(),
                PrivLength = reader.ReadUInt32()
            };
            return header;
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
