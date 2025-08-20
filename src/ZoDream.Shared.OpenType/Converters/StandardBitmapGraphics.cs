using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class StandardBitmapGraphicsConverter : TypefaceTableConverter<StandardBitmapGraphicsTable>
    {
        public override StandardBitmapGraphicsTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<MaxProfileTable>(out var profile))
            {
                return null;
            }
            var numGlyphs = profile.GlyphCount;
            var beginAt = reader.Position;
            var res = new StandardBitmapGraphicsTable();
            var version = reader.ReadUInt16();
            var flags = reader.ReadUInt16();
            var numStrikes = reader.ReadUInt32();
            var strikeOffsets = reader.ReadArray((int)numStrikes, reader.ReadUInt32);
            res.Strikes = new SbixStrike[strikeOffsets.Length];
            for (int i = 0; i < strikeOffsets.Length; i++)
            {
                res.Strikes[i] = ReadStrike(reader, numGlyphs, beginAt + strikeOffsets[i]);
            }
            return res;
        }

        private SbixStrike ReadStrike(EndianReader reader, int numGlyphs, long beginAt)
        {
            reader.Position = beginAt;
            var res = new SbixStrike()
            {
                Ppem = reader.ReadUInt16(),
                Ppi = reader.ReadUInt16(),
            };
            var offsets = reader.ReadArray(numGlyphs + 1, reader.ReadUInt32);
            res.GlyphItems = new SbixGlyphData[numGlyphs];
            for (int i = 0; i < res.GlyphItems.Length; i++)
            {
                res.GlyphItems[i] = ReadGlyph(reader, beginAt + offsets[i], beginAt + offsets[i + 1]);
            }
            return res;
        }

        private SbixGlyphData ReadGlyph(EndianReader reader, long beginAt, long endAt)
        {
            var res = new SbixGlyphData()
            {
                OriginOffsetX = reader.ReadUInt16(),
                OriginOffsetY = reader.ReadUInt16(),
                GraphicType = reader.ReadString(4),
            };
            if (res.GraphicType == "dupe")
            {
                res.GlyphId = reader.ReadUInt16();
            } else
            {
                res.Buffer = reader.ReadAsStream(endAt - reader.Position);
            }
            return res;
        }

        public override void Write(EndianWriter writer, StandardBitmapGraphicsTable data)
        {
            throw new NotImplementedException();
        }
    }
}
