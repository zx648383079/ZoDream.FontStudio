using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.WebType
{
    public class WOFFReader(EndianReader reader) : ITypefaceReader
    {
        public static byte[] Signature = "wOFF"u8.ToArray();
        public WOFFReader(Stream input) : this(new EndianReader(input, EndianType.BigEndian))
        {

        }
        public ITypefaceCollection Read()
        {
            var buffer = reader.ReadBytes(Signature.Length);
            Debug.Assert(buffer.SequenceEqual(Signature));
            var header = ReadHeader();
            var entries = ReadEntry(header).ToArray();
            foreach (var item in entries)
            {
                var source = new PartialStream(reader.BaseStream, item.Offset, item.CompLength);
                if (item.OrigLength == item.CompLength)
                {
                    // 没加密
                }
                else
                {
                    new DeflateStream(source, CompressionMode.Decompress);
                }
            }
        }

        private IEnumerable<WOFFTableEntry> ReadEntry(WOFFFileHeader header)
        {
            var expectedStartAt = 0L;
            for (int i = 0; i < header.NumTables; i++)
            {
                var entry = new WOFFTableEntry()
                {
                    Tag = reader.ReadUInt32(),
                    Offset = reader.ReadUInt32(),
                    CompLength = reader.ReadUInt32(),
                    OrigLength = reader.ReadUInt32(),
                    OrigChecksum = reader.ReadUInt32(),

                    ExpectedStartAt = expectedStartAt,
                };
                yield return entry;
                expectedStartAt += entry.OrigLength;
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
