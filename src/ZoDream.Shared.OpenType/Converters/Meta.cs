using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MetaConverter : TypefaceConverter<MetaTable>
    {
        public override MetaTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new MetaTable();

            long tableStartsAt = reader.BaseStream.Position;

            uint version = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();
            uint reserved = reader.ReadUInt32();
            uint dataMapsCount = reader.ReadUInt32();

            var dataMaps = new DataMapRecord[dataMapsCount];
            for (int i = 0; i < dataMaps.Length; ++i)
            {
                dataMaps[i] = new DataMapRecord(reader.ReadString(4),
                    reader.ReadUInt32(),
                    reader.ReadUInt32());
            }
            for (int i = 0; i < dataMaps.Length; ++i)
            {
                var record = dataMaps[i];

                switch (record.Tag)
                {
                    case "appl":
                    case "bild":
                        break;
                    case "dlng":
                        {
                            if (res.DesignLanguageTags == null)
                            {
                                reader.BaseStream.Position = tableStartsAt + record.Offset;
                                res.DesignLanguageTags = ReadCommaSepData(reader.ReadString((int)record.Length));
                            }
                        }
                        break;
                    case "slng":
                        {
                            if (res.SupportedLanguageTags == null)
                            {
                                reader.BaseStream.Position = tableStartsAt + record.Offset;
                                res.SupportedLanguageTags = ReadCommaSepData(reader.ReadString((int)record.Length));
                            }
                        }
                        break;
                }
            }
            return res;
        }

        static string[] ReadCommaSepData(string text)
        {
            var tags = text.Split(',');
            for (int i = 0; i < tags.Length; ++i)
            {
                tags[i] = tags[i].Trim();
            }
            return tags;
        }

        public override void Write(EndianWriter writer, MetaTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
