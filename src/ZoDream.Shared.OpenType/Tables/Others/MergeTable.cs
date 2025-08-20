using System;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class MergeTable : ITypefaceTable
    {

        public const string TableName = "MERG";

        public string Name => TableName;

        public MergeEntry MergeData;

        public ClassDefTable[] ClassDefs;
    }

    public class MergeEntry
    {
        public MergeEntryRow[] MergeEntryRows;
    }

    public class MergeEntryRow
    {
        /// <summary>
        /// MergeFlags
        /// </summary>
        public byte[] MergeEntries;
    }

    [Flags]
    public enum MergeFlags : byte
    {
        MERGE_LTR = 0x1,
        GROUP_LTR = 0x2,
        SECOND_IS_SUBORDINATE_LTR = 0x4,
        MERGE_RTL = 0x10,
        GROUP_RTL = 0x20,
        SECOND_IS_SUBORDINATE_RTL = 0x40
    }

}
