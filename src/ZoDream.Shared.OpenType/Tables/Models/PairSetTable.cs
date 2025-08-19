using System.IO;

namespace ZoDream.Shared.OpenType.Tables
{
    public class PairSetTable
    {
        internal PairSet[] _pairSets;
        public bool FindPairSet(ushort secondGlyphIndex, out PairSet foundPairSet)
        {
            int j = _pairSets.Length;
            for (int i = 0; i < j; ++i)
            {
                //TODO: binary search?
                if (_pairSets[i].secondGlyph == secondGlyphIndex)
                {
                    //found
                    foundPairSet = _pairSets[i];
                    return true;
                }
            }
            //
            foundPairSet = new PairSet();//empty
            return false;
        }
    }
}
