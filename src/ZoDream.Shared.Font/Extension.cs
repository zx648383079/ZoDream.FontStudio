using System.Collections.Generic;

namespace ZoDream.Shared.Font
{
    public static class Extension
    {
        public static ITypefaceTableCollection ToCollection(this IEnumerable<ITypefaceTable> items)
        {
            if (items is ITypefaceTableCollection c)
            {
                return c;
            }
            var res = new TypefaceTableCollection();
            foreach (var item in items)
            {
                res.Add(item);
            }
            return res;
        }

        public static ITypefaceTableCollection ToCollection(this IEnumerable<ITypefaceTableEntry> items)
        {
            var res = new TypefaceTableCollection();
            foreach (var item in items)
            {
                res.Add(new UnreadTypefaceTable(item));
            }
            return res;
        }
    }
}
