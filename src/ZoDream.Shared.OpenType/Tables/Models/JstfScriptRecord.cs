namespace ZoDream.Shared.OpenType.Tables
{
    internal readonly struct JstfScriptRecord
    {
        public readonly string Tag;
        public readonly ushort Offset;
        public JstfScriptRecord(string jstfScriptTag, ushort jstfScriptOffset)
        {
            Tag = jstfScriptTag;
            Offset = jstfScriptOffset;
        }
    }
}
