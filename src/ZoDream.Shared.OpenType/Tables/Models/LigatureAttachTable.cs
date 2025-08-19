namespace ZoDream.Shared.OpenType.Tables
{
    public class LigatureAttachTable
    {
        public ComponentRecord[] _records;

        public ComponentRecord GetComponentRecord(int index) => _records[index];
    }
}