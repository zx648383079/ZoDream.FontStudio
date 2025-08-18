using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class KerningSubTable
    {
        List<KerningPair> _kernPairs;
        Dictionary<uint, short> _kernDic;
        public KerningSubTable(int capcity)
        {
            _kernPairs = new List<KerningPair>(capcity);
            _kernDic = new Dictionary<uint, short>(capcity);
        }

        public void AddKernPair(ushort left, ushort right, short value)
        {
            _kernPairs.Add(new KerningPair(left, right, value));
            //may has duplicate key ?
            //TODO: review here
            uint key = (uint)((left << 16) | right);
            _kernDic[key] = value; //just replace?                 
        }
        public short GetKernDistance(ushort left, ushort right)
        {
            //find if we have this left & right ?
            uint key = (uint)((left << 16) | right);

            _kernDic.TryGetValue(key, out short found);
            return found;
        }
    }
}
