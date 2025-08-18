namespace ZoDream.Shared.OpenType.Tables
{
    readonly struct BaseScriptRecord
    {
        public readonly string BaseScriptTag;
        public readonly ushort BaseScriptOffset;
        public BaseScriptRecord(string scriptTag, ushort offset)
        {
            BaseScriptTag = scriptTag;
            BaseScriptOffset = offset;
        }
    }
}
