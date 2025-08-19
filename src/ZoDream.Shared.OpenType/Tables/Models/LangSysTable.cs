namespace ZoDream.Shared.OpenType.Tables
{
    public class LangSysTable
    {
        public string Tag { get; set; }
        internal readonly ushort Offset;

        //
        public ushort[] featureIndexList { get; set; }
        public ushort RequiredFeatureIndex { get; set; }

        public LangSysTable(string tag, ushort offset)
        {
            Offset = offset;
            Tag = tag;
        }
    }
}