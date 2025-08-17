using System;
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
            var offset32 = reader.ReadUInt32();
            var reserved = reader.ReadUInt32();

            var docPos = pos + offset32;
            reader.Position = docPos;
            var numEntries = reader.ReadUInt16();
            res.Bodies = reader.ReadArray(numEntries, () => {
                var doc = new SvgDocument()
                {
                    StartGlyphID = reader.ReadUInt16(),
                    EndGlyphID = reader.ReadUInt16(),
                    SvgDocOffset = reader.ReadUInt32(),
                    SvgDocLength = reader.ReadUInt32(),
                };
                doc.Buffer = new PartialStream(reader.BaseStream, docPos + doc.SvgDocOffset, doc.SvgDocLength);
                // 根据 UTF8 buffer 第一个字符是否是 < 判断是否压缩
                
                return doc;
            });

            return res;
        }

        public override void Write(EndianWriter writer, SvgTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
