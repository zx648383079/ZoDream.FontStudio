using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class LinearThresholdConverter : TypefaceTableConverter<LinearThresholdTable>
    {
        public override LinearThresholdTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<MaxProfileTable>(out var profile))
            {
                return null;
            }
            var res = new LinearThresholdTable();
            var version = reader.ReadUInt16();
            var numGlyphs = profile.GlyphCount;
            res.yPixels = reader.ReadArray(numGlyphs, reader.ReadByte);
            return res;
        }

        public override void Write(EndianWriter writer, LinearThresholdTable data)
        {
            throw new NotImplementedException();
        }
    }
}
