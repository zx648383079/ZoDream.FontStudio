using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class FontProgramTable : ITypefaceTable
    {
        public const string TableName = "fpgm";

        public string Name => TableName;
    }
}
