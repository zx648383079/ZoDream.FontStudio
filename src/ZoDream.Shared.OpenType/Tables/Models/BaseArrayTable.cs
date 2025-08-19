namespace ZoDream.Shared.OpenType.Tables
{
    public class BaseArrayTable
    {
        internal BaseRecord[] _records;

        public BaseRecord GetBaseRecords(int index)
        {
            return _records[index];
        }
    }
}