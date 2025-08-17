using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class HorizontalMetricsTable : ITypefaceTable
    {
        public const string TableName = "hmtx";

        public string Name => TableName;
        /// <summary>
        /// in font design unit
        /// </summary>
        public ushort[] AdvanceWidths { get; set; }
        /// <summary>
        /// lsb, in font design unit
        /// </summary>
        public short[] LeftSideBearings { get; set; }
    }
}
