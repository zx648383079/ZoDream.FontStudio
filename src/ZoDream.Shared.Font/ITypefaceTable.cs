using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceTableEntry
    {
        public string Name { get; }
    }

    public interface ITypefaceTable
    {
        public string Name { get; }
    }

    public interface ITableCollection : IEnumerable<ITypefaceTable>
    {

        public void Add(ITypefaceTable table);
        public bool TryGet(string name, [NotNullWhen(true)] out ITypefaceTable? res);
    }

    public class TableCollection : Dictionary<string, ITypefaceTable>, ITableCollection
    {
        public void Add(ITypefaceTable table)
        {
            Add(table.Name, table);
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
