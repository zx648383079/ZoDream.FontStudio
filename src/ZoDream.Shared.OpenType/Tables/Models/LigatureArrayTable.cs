namespace ZoDream.Shared.OpenType.Tables
{
    public class LigatureArrayTable
    {
        public LigatureAttachTable[] _ligatures;

        public LigatureAttachTable GetLigatureAttachTable(int index) => _ligatures[index];
    }
}