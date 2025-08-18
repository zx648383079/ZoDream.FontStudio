namespace ZoDream.Shared.OpenType.Tables
{
    public class InstanceRecord
    {
        public ushort SubfamilyNameID;//point to name table, will be resolved later
        public ushort Flags;
        public float[] Coordinates;
        public ushort PostScriptNameID;//point to name table, will be resolved later
    }
}
