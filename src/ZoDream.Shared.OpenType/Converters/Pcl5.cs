using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class Pcl5Converter : TypefaceConverter<Pcl5Table>
    {
        public override Pcl5Table? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new Pcl5Table();
            var majorVersion  = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            res.FontNumber = reader.ReadUInt32();
            res.Pitch = reader.ReadUInt16();
            res.XHeight = reader.ReadUInt16();
            res.Style  = reader.ReadUInt16();
            res.TypeFamily  = reader.ReadUInt16();
            res.CapHeight = reader.ReadUInt16();
            res.SymbolSet  = reader.ReadUInt16();
            res.Typeface = reader.ReadBytes(16);
            res.CharacterComplement = reader.ReadBytes(8);
            res.FileName = reader.ReadBytes(6);
            res.StrokeWeight = reader.ReadSByte();
            res.WidthType = reader.ReadSByte();
            res.SerifStyle = reader.ReadByte();
            _ = reader.ReadByte();
            return res;
        }

        public override void Write(EndianWriter writer, Pcl5Table data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
