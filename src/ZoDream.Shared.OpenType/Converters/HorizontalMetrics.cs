using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class HorizontalMetricsConverter : TypefaceTableConverter<HorizontalMetricsTable>
    {
        public override HorizontalMetricsTable? Read(EndianReader reader, 
            ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<HorizontalHeaderTable>(out var header)
                || !serializer.TryGet<MaxProfileTable>(out var profile))
            {
                return null;
            }
            var res = new HorizontalMetricsTable()
            {
                AdvanceWidths = new ushort[profile.GlyphCount],
                LeftSideBearings = new short[profile.GlyphCount],
            };
            var numOfHMetrics = header.NumberOfHMetrics;
            var gid = 0; //gid=> glyphIndex
            for (int i = 0; i < numOfHMetrics; i++)
            {
                res.AdvanceWidths[gid] = reader.ReadUInt16();
                res.LeftSideBearings[gid] = reader.ReadInt16();

                gid++;//***
            }
            var nEntries = profile.GlyphCount - numOfHMetrics;
            var advanceWidth = res.AdvanceWidths[numOfHMetrics - 1];

            for (int i = 0; i < nEntries; i++)
            {
                res.AdvanceWidths[gid] = advanceWidth;
                res.LeftSideBearings[gid] = reader.ReadInt16();

                gid++;//***
            }
            return res;
        }

        public override void Write(EndianWriter writer, HorizontalMetricsTable data)
        {
            throw new System.NotImplementedException();
        }
    }
}
