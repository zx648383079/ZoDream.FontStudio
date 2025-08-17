using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ControlValueTable : ITypefaceTable
    {
        public const string TableName = "cvt ";

        public string Name => TableName;

        public short[] ControlValues { get; set; }
    }
}
