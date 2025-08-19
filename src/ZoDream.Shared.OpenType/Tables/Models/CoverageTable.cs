using System;
using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class CoverageTable
    {
        public abstract int FindPosition(ushort glyphIndex);
        public abstract IEnumerable<ushort> GetExpandedValueIter();
    }

    public class CoverageFmt1 : CoverageTable
    {
        internal ushort[] OrderedGlyphIdList;


        public override int FindPosition(ushort glyphIndex)
        {
            // "The glyph indices must be in numerical order for binary searching of the list"
            // (https://www.microsoft.com/typography/otspec/chapter2.htm#coverageFormat1)
            int n = Array.BinarySearch(OrderedGlyphIdList, glyphIndex);
            return n < 0 ? -1 : n;
        }
        public override IEnumerable<ushort> GetExpandedValueIter() { return OrderedGlyphIdList; }
    }

    public class CoverageFmt2 : CoverageTable
    {
        internal ushort[] StartIndices;
        internal ushort[] EndIndices;
        internal ushort[] CoverageIndices;

        public override int FindPosition(ushort glyphIndex)
        {
            // Ranges must be in glyph ID order, and they must be distinct, with no overlapping.
            // [...] quick calculation of the Coverage Index for any glyph in any range using the
            // formula: Coverage Index (glyphID) = startCoverageIndex + glyphID - startGlyphID.
            // (https://www.microsoft.com/typography/otspec/chapter2.htm#coverageFormat2)
            int n = Array.BinarySearch(EndIndices, glyphIndex);
            n = n < 0 ? ~n : n;
            if (n >= StartIndices.Length || glyphIndex < StartIndices[n])
            {
                return -1;
            }
            return CoverageIndices[n] + glyphIndex - StartIndices[n];
        }

        public override IEnumerable<ushort> GetExpandedValueIter()
        {
            for (int i = 0; i < StartIndices.Length; ++i)
            {
                for (ushort n = StartIndices[i]; n <= EndIndices[i]; ++n)
                {
                    yield return n;
                }
            }
        }
    }
}
