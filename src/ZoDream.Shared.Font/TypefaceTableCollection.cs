using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ZoDream.Shared.Font
{
    public class TypefaceTableCollection : Dictionary<string, ITypefaceTable>, ITypefaceTableCollection
    {
        public void Add(ITypefaceTable table)
        {
            if (TryAdd(table.Name, table))
            {
                return;
            }
            this[table.Name] = table;
        }

        public bool TryGet(string name, [NotNullWhen(true)] out ITypefaceTable? res)
        {
            return TryGetValue(name, out res);
        }

        IEnumerator<ITypefaceTable> IEnumerable<ITypefaceTable>.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}
