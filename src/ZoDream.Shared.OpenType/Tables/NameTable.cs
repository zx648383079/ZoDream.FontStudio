using System.Collections.Generic;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class NameTable : ITypefaceTable
    {
        public const string TableName = "name";

        public string Name => TableName;

        public Dictionary<NameID, string> Items { get; set; } = [];
    }



}
