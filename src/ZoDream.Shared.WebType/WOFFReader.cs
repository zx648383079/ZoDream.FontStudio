using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType;

namespace ZoDream.Shared.WebType
{
    public class WOFFReader(EndianReader reader) : ITypefaceReader, ITypefaceCipher
    {
        public static byte[] Signature = "wOFF"u8.ToArray();
        public WOFFReader(Stream input) : this(new EndianReader(input, EndianType.BigEndian, false))
        {

        }
        public ITypefaceCollection Read()
        {
            var buffer = reader.ReadBytes(Signature.Length);
            Debug.Assert(buffer.SequenceEqual(Signature));
            var header = ReadHeader();
            var entries = ReadEntry(header).ToArray();
            var res = new Typeface();
            var data = entries.ToCollection();
            var serializer = new TypefaceTableSerializer(reader.BaseStream, 
                new TypefaceSerializer(OTFReader.Converters), data, this);
            foreach (var item in entries)
            {
                serializer.TryGet(item, out _);
            }
            return new TypefaceCollection
            {
                res
            };
        }

        public Stream Encrypt(Stream input)
        {
            return input;
        }

        public Stream Decrypt(Stream input, ITypefaceTableEntry entry)
        {
            if (entry is not WOFFTableEntry item)
            {
                return new PartialStream(input, entry.Offset, entry.Length);
            }
            var source = new PartialStream(reader.BaseStream, item.CompressedOffset, item.CompressedLength);
            if (item.Length == item.CompressedLength)
            {
                // 没加密
                return source;
            }
            var ms = new MemoryStream((int)item.Length);
            new DeflateStream(source, CompressionMode.Decompress).CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        private IEnumerable<WOFFTableEntry> ReadEntry(WOFFFileHeader header)
        {
            for (int i = 0; i < header.NumTables; i++)
            {
                var entry = new WOFFTableEntry()
                {
                    Name = reader.ReadString(4),
                    CompressedOffset = reader.ReadUInt32(),
                    CompressedLength = reader.ReadUInt32(),
                    Length = reader.ReadUInt32(),
                    OriginalChecksum = reader.ReadUInt32(),
                };
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
