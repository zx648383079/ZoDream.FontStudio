using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class LookupSubTable
    {
        public GlyphPositioningTable OwnerGPos { get; internal set; }
        public GlyphSubstitutionTable OwnerGSub { get; internal set; }

        public virtual void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {

        }

        public virtual bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            return false;
        }

        public virtual void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {

        }

        internal static int FindGlyphBackwardByKind(IGlyphPositions inputGlyphs, GlyphClassKind kind, int pos, int lim)
        {
            for (int i = pos; --i >= lim;)
            {
                if (inputGlyphs.GetGlyphClassKind(i) == kind)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public class UnImplementedLookupSubTable(string message) : LookupSubTable
    {

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len) { }
    }
}
