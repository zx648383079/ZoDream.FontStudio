using System;
using System.Text;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class NameConverter : TypefaceConverter<NameTable>
    {
        public override NameTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var pos = reader.Position;
            var res = new NameTable();
            var uFSelector = reader.ReadUInt16();
            var uNRCount = reader.ReadUInt16();
            var uStorageOffset = reader.ReadUInt16();
            var offsetItems = reader.ReadArray(uNRCount, () => ReadRecord(reader));
            foreach (var item in offsetItems)
            {
                reader.Position = pos + item.StringOffset + uStorageOffset;
                var encoding = item.EncodingID is 3 or 1 ? Encoding.BigEndianUnicode : Encoding.UTF8;
                var text = encoding.GetString(reader.ReadBytes(item.StringLength));
                res.Items.TryAdd((NameID)item.NameID, text);
            }
            return res;
        }

        private NameRecord ReadRecord(EndianReader reader)
        {
            return new NameRecord()
            {
                PlatformID = reader.ReadUInt16(),
                EncodingID = reader.ReadUInt16(),
                LanguageID = reader.ReadUInt16(),
                NameID = reader.ReadUInt16(),
                StringLength = reader.ReadUInt16(),
                StringOffset = reader.ReadUInt16(),
            };
        }

        public override void Write(EndianWriter writer, NameTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
