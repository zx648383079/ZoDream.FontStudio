namespace ZoDream.Shared.Font
{
    public class UnreadTypefaceTable(ITypefaceTableEntry entry) : IUnreadTypefaceTable
    {
        public ITypefaceTableEntry Entry => entry;

        public string Name => entry.Name;
    }
}
