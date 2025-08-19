using System;
using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LookupTable
    {
        public readonly ushort lookupFlags;
        public readonly ushort markFilteringSet;
        public LookupSubTable[] SubTables { get; internal set; }
        public LookupTable(ushort lookupFlags, ushort markFilteringSet)
        {
            this.lookupFlags = lookupFlags;
            this.markFilteringSet = markFilteringSet;
        }

        public bool DoSubstitutionAt(IGlyphIndexList inputGlyphs, int pos, int len)
        {
            foreach (LookupSubTable subTable in SubTables)
            {
                // We return after the first substitution, as explained in the spec:
                // "A lookup is finished for a glyph after the client locates the target
                // glyph or glyph context and performs a substitution, if specified."
                // https://www.microsoft.com/typography/otspec/gsub.htm
                if (subTable.DoSubstitutionAt(inputGlyphs, pos, len))
                    return true;
            }
            return false;
        }

        public void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            foreach (LookupSubTable subTable in SubTables)
            {
                subTable.DoGlyphPosition(inputGlyphs, startAt, len);
                //update len
                len = inputGlyphs.Count;
            }
        }

        public void CollectAssociatedSubstitutionGlyph(List<ushort> outputAssocGlyphs)
        {

            foreach (LookupSubTable subTable in SubTables)
            {
                subTable.CollectAssociatedSubtitutionGlyphs(outputAssocGlyphs);
            }
        }
    }
}
