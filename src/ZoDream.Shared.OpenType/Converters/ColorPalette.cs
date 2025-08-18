using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ColorPaletteConverter : TypefaceConverter<ColorPaletteTable>
    {
        public override ColorPaletteTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new ColorPaletteTable();
            long beginAt = reader.BaseStream.Position;
            ushort version = reader.ReadUInt16();
            ushort numPaletteEntries = reader.ReadUInt16(); // XXX: unused?
            ushort numPalettes = reader.ReadUInt16();
            var colorCount = reader.ReadUInt16();           //numColorRecords
            uint offsetFirstColorRecord = reader.ReadUInt32();   //Offset from the beginning of CPAL table to the first ColorRecord.
            res.Palettes = reader.ReadArray(numPalettes, reader.ReadUInt16); //colorRecordIndices, Index of each palette’s first color record in the combined color record array.


            reader.BaseStream.Seek(beginAt + offsetFirstColorRecord, SeekOrigin.Begin);
            res.Colors = reader.ReadArray(colorCount, () => {
                return Color.FromBGRA(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            });
            return res;
        }

        public override void Write(EndianWriter writer, ColorPaletteTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
