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

    public class VariableAxisRecord
    {
        public string AxisTag;
        public float MinValue;
        public float DefaultValue;
        public float MaxValue;
        public ushort Flags;
        public ushort AxisNameID;
    }

    public class InstanceRecord
    {
        public ushort SubfamilyNameID;//point to name table, will be resolved later
        public ushort Flags;
        public float[] Coordinates;
        public ushort PostScriptNameID;//point to name table, will be resolved later
    }
}
