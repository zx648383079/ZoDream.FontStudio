using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceTableEntry
    {
        public string Name { get; }

        public long Offset { get; }
        public long Length { get; }
    }

    public interface ITypefaceTable
    {
        public string Name { get; }
    }

    public interface IUnreadTypefaceTable : ITypefaceTable
    {
        public ITypefaceTableEntry Entry { get; }
    }

    public interface ITypefaceTableCollection : IEnumerable<ITypefaceTable>
    {
        public void Add(ITypefaceTable table);
        public bool TryGet(string name, [NotNullWhen(true)] out ITypefaceTable? res);
    }
}
