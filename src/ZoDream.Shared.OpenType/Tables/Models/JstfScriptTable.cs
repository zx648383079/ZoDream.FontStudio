namespace ZoDream.Shared.OpenType.Tables
{
    public class JstfScriptTable
    {
        public ushort[] ExtenderGlyphs;

        public JstfLangSysRecord DefaultLangSys;
        public JstfLangSysRecord[] Other;

        public string ScriptTag { get; set; }
    }
}
