using System;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType4 : LookupSubTable
    {
        public CoverageTable MarkCoverageTable { get; set; }
        public CoverageTable BaseCoverageTable { get; set; }
        public BaseArrayTable BaseArrayTable { get; set; }
        public MarkArrayTable MarkArrayTable { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            int lim = Math.Min(startAt + len, inputGlyphs.Count);

            // Find the mark glyph, starting at 1
            bool longLookBack = this.OwnerGPos.EnableLongLookBack;
            for (int i = Math.Max(startAt, 1); i < lim; ++i)
            {
                int markFound = MarkCoverageTable.FindPosition(inputGlyphs.GetGlyph(i, out short glyph_advW));
                if (markFound < 0)
                {
                    continue;
                }

                // Look backwards for the base glyph
                int j = FindGlyphBackwardByKind(inputGlyphs, GlyphClassKind.Base, i, longLookBack ? startAt : i - 1);
                if (j < 0)
                {
                    // Fall back to type 0
                    j = FindGlyphBackwardByKind(inputGlyphs, GlyphClassKind.Zero, i, longLookBack ? startAt : i - 1);
                    if (j < 0)
                    {
                        continue;
                    }
                }

                ushort prev_glyph = inputGlyphs.GetGlyph(j, out short prev_glyph_adv_w);
                int baseFound = BaseCoverageTable.FindPosition(prev_glyph);
                if (baseFound < 0)
                {
                    continue;
                }

                BaseRecord baseRecord = BaseArrayTable.GetBaseRecords(baseFound);
                ushort markClass = MarkArrayTable.GetMarkClass(markFound);
                // find anchor on base glyph
                AnchorPoint anchor = MarkArrayTable.GetAnchorPoint(markFound);
                AnchorPoint prev_anchor = baseRecord.anchors[markClass];
                inputGlyphs.GetOffset(j, out short prev_glyph_xoffset, out short prev_glyph_yoffset);
                inputGlyphs.GetOffset(i, out short glyph_xoffset, out short glyph_yoffset);
                int xoffset = prev_glyph_xoffset + prev_anchor.xcoord - (prev_glyph_adv_w + glyph_xoffset + anchor.xcoord);
                int yoffset = prev_glyph_yoffset + prev_anchor.ycoord - (glyph_yoffset + anchor.ycoord);
                inputGlyphs.AppendGlyphOffset(i, (short)xoffset, (short)yoffset);
            }
        }
    }
}
