using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class HeadConverter : TypefaceConverter<HeadTable>
    {
        public override HeadTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new HeadTable();

            res.Version = reader.ReadUInt32(); // 0x00010000 for version 1.0.
            res.FontRevision = reader.ReadUInt32();
            res.CheckSumAdjustment = reader.ReadUInt32();
            res.MagicNumber = reader.ReadUInt32();
            if (res.MagicNumber != 0x5F0F3CF5)
            {
                throw new Exception("Invalid magic number!" + res.MagicNumber.ToString("x"));
            }

            res.Flags = reader.ReadUInt16();
            res.UnitsPerEm = reader.ReadUInt16(); // valid is 16 to 16384
            res.Created = reader.ReadUInt64(); //  International date (8-byte field). (?)
            res.Modified = reader.ReadUInt64();
            // bounding box for all glyphs
            res.Bounds = reader.ReadBounds();
            res.MacStyle = reader.ReadUInt16();
            res.LowestRecPPEM = reader.ReadUInt16();
            res.FontDirectionHint = reader.ReadInt16();
            res.IndexToLocFormat = reader.ReadInt16(); // 0 for 16-bit offsets, 1 for 32-bit.
            res.GlyphDataFormat = reader.ReadInt16(); // 0
            return res;
        }

        public override void Write(EndianWriter writer, HeadTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
