using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables
{
    public class LkSubTableT5Fmt2 : LookupSubTable
    {
        //Format 2, a more flexible format than Format 1,
        //describes class-based context substitution.
        //For this format, a specific integer, called a class value, must be assigned to each glyph component in all context glyph sequences.
        //Contexts are then defined as sequences of glyph class values.
        //More than one context may be defined at a time.

        public CoverageTable coverageTable;
        public ClassDefTable classDef;
        public LkSubT5Fmt2_SubClassSet[] subClassSets;

        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            //collect only assoc  
            Dictionary<int, bool> collected = new Dictionary<int, bool>();
            foreach (ushort glyphIndex in coverageTable.GetExpandedValueIter())
            {
                int class_value = classDef.GetClassValue(glyphIndex);
                if (collected.ContainsKey(class_value))
                {
                    continue;
                }
                //
                collected.Add(class_value, true);

                LkSubT5Fmt2_SubClassSet subClassSet = subClassSets[class_value];
                LkSubT5Fmt2_SubClassRule[] subClassRules = subClassSet.subClassRules;

                for (int i = 0; i < subClassRules.Length; ++i)
                {
                    LkSubT5Fmt2_SubClassRule rule = subClassRules[i];
                    if (rule != null && rule.substRecords != null)
                    {
                        for (int n = 0; n < rule.substRecords.Length; ++n)
                        {
                            SubstLookupRecord rect = rule.substRecords[n];
                            LookupTable anotherLookup = OwnerGSub.LookupList[rect.lookupListIndex];
                            anotherLookup.CollectAssociatedSubstitutionGlyph(outputAssocGlyphs);
                        }
                    }
                }
            }
        }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int coverage_pos = coverageTable.FindPosition(glyphIndices[pos]);
            if (coverage_pos < 0) { return false; }

            int class_value = classDef.GetClassValue(glyphIndices[pos]);

            LkSubT5Fmt2_SubClassSet subClassSet = subClassSets[class_value];
            LkSubT5Fmt2_SubClassRule[] subClassRules = subClassSet.subClassRules;

            for (int i = 0; i < subClassRules.Length; ++i)
            {
                LkSubT5Fmt2_SubClassRule rule = subClassRules[i];
                ushort[] inputSequence = rule.inputSequence; //clas seq
                int next_pos = pos + 1;


                if (next_pos < glyphIndices.Count)
                {
                    bool passAll = true;
                    for (int a = 0; a < inputSequence.Length && next_pos < glyphIndices.Count; ++a, ++next_pos)
                    {
                        int class_next = glyphIndices[next_pos];
                        if (inputSequence[a] != class_next)
                        {
                            passAll = false;
                            break;
                        }
                    }
                    if (passAll)
                    {

                    }
                }
            }

            return false;
        }
    }
}
