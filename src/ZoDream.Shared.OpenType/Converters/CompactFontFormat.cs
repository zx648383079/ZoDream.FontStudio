using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public partial class CompactFontFormatConverter : TypefaceConverter<CompactFontFormatTable>
    {
        public override CompactFontFormatTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new CompactFontFormatTable();
            long startAt = reader.BaseStream.Position;
            byte major = reader.ReadByte();
            byte minor = reader.ReadByte();
            byte hdrSize = reader.ReadByte();
            byte offSize = reader.ReadByte();
            ////---------
            //name index

            switch (major)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        Cff1Parser cff1 = new Cff1Parser();
                        cff1.ParseAfterHeader(startAt, reader);
                        _cff1FontSet = cff1.ResultCff1FontSet;
                    }
                    break;
                case 2:
                    {
                        Cff2Parser cff2 = new Cff2Parser();
                        cff2.ParseAfterHeader(reader);
                    }
                    break;
            }
            return res;
        }



        public override void Write(EndianWriter writer, CompactFontFormatTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
