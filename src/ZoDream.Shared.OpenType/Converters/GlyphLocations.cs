using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GlyphLocationsConverter : TypefaceTableConverter<GlyphLocationsTable>
    {
        public override GlyphLocationsTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<MaxProfileTable>(out var profile)
                || !serializer.TryGet<HeadTable>(out var head))
            {
                return null;
            }
            var res = new GlyphLocationsTable();
            int glyphCount = profile.GlyphCount;
            int lim = glyphCount + 1;
            res.Offsets = new uint[lim];
            if (head.WideGlyphLocations)
            {
                //long version
                for (int i = 0; i < lim; i++)
                {
                    res.Offsets[i] = reader.ReadUInt32();
                }
            }
            else
            {
                //short version
                for (int i = 0; i < lim; i++)
                {
                    res.Offsets[i] = (uint)(reader.ReadUInt16() << 1); // =*2
                }
            }

            return res;
        }

        public override void Write(EndianWriter writer, GlyphLocationsTable data)
        {
            throw new NotImplementedException();
        }
    }
}
