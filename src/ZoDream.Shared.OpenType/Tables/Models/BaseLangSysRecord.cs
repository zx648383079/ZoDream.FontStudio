namespace ZoDream.Shared.OpenType.Tables
{
    public readonly struct BaseLangSysRecord
    {
        public readonly string BaseScriptTag;
        public readonly ushort BaseScriptOffset;
        public BaseLangSysRecord(string scriptTag, ushort offset)
        {
            BaseScriptTag = scriptTag;
            BaseScriptOffset = offset;
        }
    }
}
