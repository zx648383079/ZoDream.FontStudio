using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableType7Fmt2 : LookupSubTable
    {
        public ClassDefTable ClassDef { get; set; }
        public CoverageTable CoverageTable { get; set; }
        public PosClassSetTable[] PosClassSetTables { get; set; }
        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            int lim = Math.Min(startAt + len, inputGlyphs.Count);
            for (int i = startAt; i < lim; ++i)
            {
                ushort glyph1_index = inputGlyphs.GetGlyph(i, out short unused);
                if (CoverageTable.FindPosition(glyph1_index) < 0)
                {
                    continue;
                }

                int glyph1_class = ClassDef.GetClassValue(glyph1_index);
                if (glyph1_class >= PosClassSetTables.Length || PosClassSetTables[glyph1_class] == null)
                {
                    continue;
                }

                foreach (PosClassRule rule in PosClassSetTables[glyph1_class].PosClassRules)
                {
                    ushort[] glyphIds = rule.InputGlyphIds;
                    int matches = 0;
                    for (int n = 0; n < glyphIds.Length && i + 1 + n < lim; ++n)
                    {
                        ushort glyphn_index = inputGlyphs.GetGlyph(i + 1 + n, out unused);
                        int glyphn_class = ClassDef.GetClassValue(glyphn_index);
                        if (glyphn_class != glyphIds[n])
                        {
                            break;
                        }
                        ++matches;
                    }

                    if (matches == glyphIds.Length)
                    {
                        foreach (PosLookupRecord plr in rule.PosLookupRecords)
                        {
                            LookupTable lookup = OwnerGPos.LookupList[plr.lookupListIndex];
                            lookup.DoGlyphPosition(inputGlyphs, i + plr.seqIndex, glyphIds.Length - plr.seqIndex);
                        }
                        break;
                    }
                }
            }
        }
    }
}
