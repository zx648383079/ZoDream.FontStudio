using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType6 : LookupSubTable
    {
        public CoverageTable MarkCoverage1 { get; set; }
        public CoverageTable MarkCoverage2 { get; set; }
        public MarkArrayTable Mark1ArrayTable { get; set; }
        public Mark2ArrayTable Mark2ArrayTable { get; set; } // Mark2 attachment points used to attach Mark1 glyphs to a specific Mark2 glyph. 

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            //The attaching mark is Mark1, 
            //and the base mark being attached to is Mark2.

            //The Mark2 glyph (that combines with a Mark1 glyph) is the glyph preceding the Mark1 glyph in glyph string order 
            //(skipping glyphs according to LookupFlags)

            //@prepare: we must found mark2 glyph before mark1
            bool longLookBack = this.OwnerGPos.EnableLongLookBack;
#if DEBUG
            if (len == 3 || len == 4)
            {

            }
#endif
            //find marker
            int lim = Math.Min(startAt + len, inputGlyphs.Count);

            for (int i = Math.Max(startAt, 1); i < lim; ++i)
            {
                // Find first mark glyph
                int mark1Found = MarkCoverage1.FindPosition(inputGlyphs.GetGlyph(i, out short glyph_adv_w));
                if (mark1Found < 0)
                {
                    continue;
                }

                // Look back for previous mark glyph
                int prev_mark = FindGlyphBackwardByKind(inputGlyphs, GlyphClassKind.Mark, i, longLookBack ? startAt : i - 1);
                if (prev_mark < 0)
                {
                    continue;
                }

                int mark2Found = MarkCoverage2.FindPosition(inputGlyphs.GetGlyph(prev_mark, out short prev_pos_adv_w));
                if (mark2Found < 0)
                {
                    continue;
                }

                // Examples:
                // 👨🏻‍👩🏿‍👧🏽‍👦🏽‍👦🏿 in Segoe UI Emoji

                int mark1ClassId = Mark1ArrayTable.GetMarkClass(mark1Found);
                AnchorPoint prev_anchor = Mark2ArrayTable.GetAnchorPoint(mark2Found, mark1ClassId);
                AnchorPoint anchor = Mark1ArrayTable.GetAnchorPoint(mark1Found);
                if (anchor.ycoord < 0)
                {
                    //temp HACK!   น้ำ in Tahoma
                    inputGlyphs.AppendGlyphOffset(prev_mark /*PREV*/, anchor.xcoord, anchor.ycoord);
                }
                else
                {
                    inputGlyphs.GetOffset(prev_mark, out short prev_glyph_xoffset, out short prev_glyph_yoffset);
                    inputGlyphs.GetOffset(i, out short glyph_xoffset, out short glyph_yoffset);
                    int xoffset = prev_glyph_xoffset + prev_anchor.xcoord - (prev_pos_adv_w + glyph_xoffset + anchor.xcoord);
                    int yoffset = prev_glyph_yoffset + prev_anchor.ycoord - (glyph_yoffset + anchor.ycoord);
                    inputGlyphs.AppendGlyphOffset(i, (short)xoffset, (short)yoffset);
                }
            }
        }
    }
}
