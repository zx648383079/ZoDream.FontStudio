using System.IO;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ControlValueProgramTable : ITypefaceTable
    {
        public const string TableName = "prep";

        public string Name => TableName;

        public Stream Buffer { get; set; }
    }
}
