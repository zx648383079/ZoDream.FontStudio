using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;
using BaseConverter = ZoDream.Shared.OpenType.Converters.GlyphLocationsConverter;

namespace ZoDream.Shared.WebType.Converters
{
    public class GlyphLocationsConverter : BaseConverter
    {
        public override GlyphLocationsTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (entry is not WOFFTableEntry t || t.OriginalLength == 0)
            {
                return base.Read(reader, entry, serializer);
            }
            return new GlyphLocationsTable();
        }
    }
}
