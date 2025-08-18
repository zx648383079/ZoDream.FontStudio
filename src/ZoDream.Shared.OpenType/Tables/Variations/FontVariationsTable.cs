using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class FontVariationsTable : ITypefaceTable
    {
        public const string TableName = "fvar";

        public string Name => TableName;

        public VariableAxisRecord[] VariableAxisRecords { get; set; }
        public InstanceRecord[] InstanceRecords { get; set; }
    }




}
