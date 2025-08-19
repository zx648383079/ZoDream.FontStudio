using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ScriptTable
    {
        public string Name { get; internal set; }
        public LangSysTable defaultLang { get; set; }// be NULL
        public LangSysTable[] langSysTables { get; set; }

    }

    public class ScriptList : Dictionary<string, ScriptTable>
    {

    }
}
