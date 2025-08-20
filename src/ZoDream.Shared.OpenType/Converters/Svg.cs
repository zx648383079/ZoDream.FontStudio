using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class SvgConverter : TypefaceConverter<SvgTable>
    {
        public override SvgTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var pos = reader.Position;
            var res = new SvgTable();
            var version = reader.ReadUInt16();
            var offset = reader.ReadUInt32();
            var reserved = reader.ReadUInt32();

            var docPos = pos + offset;
            reader.Position = docPos;
            var numEntries = reader.ReadUInt16();
            var maxLength = 0;
            res.Bodies = reader.ReadArray(numEntries, () => {
                var doc = new SvgDocument()
                {
                    StartGlyphID = reader.ReadUInt16(),
                    EndGlyphID = reader.ReadUInt16(),
                    Offset = reader.ReadUInt32(),
                    Length = (int)reader.ReadUInt32(),
                };
                maxLength = Math.Max(maxLength, doc.Length);
                return doc;
            });
            var buffer = ArrayPool<byte>.Shared.Rent(maxLength);
            foreach (var item in res.Bodies)
            {
                reader.Position = docPos + item.Offset;
                reader.Read(buffer, 0, item.Length);
                if (buffer[0] == 0x1F && buffer[1] == 0x8B && buffer[2] == 0x08)
                {
                    item.Text = Encoding.UTF8.GetString(new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress).ToArray());
                }
                else
                {
                    item.Text = Encoding.UTF8.GetString(buffer, 0, item.Length);
                }
            }
            ArrayPool<byte>.Shared.Return(buffer);
            return res;
        }

        public override void Write(EndianWriter writer, SvgTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
