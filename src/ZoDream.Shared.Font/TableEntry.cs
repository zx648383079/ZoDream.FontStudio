using System.Collections.Generic;

namespace ZoDream.Shared.Font
{

    public interface ITableEntry
    {
        public string Name { get; }
    }

    public abstract class TableEntry : ITableEntry
    {
        public TableHeader Header;

        public virtual string Name => Header.Tag;
    }

    public class TableCollection : IEnumerable<ITableEntry>
    {
    }
}
