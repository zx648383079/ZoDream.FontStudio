using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphDefinitionTable : ITypefaceTable
    {
        public const string TableName = "GDEF";

        public string Name => TableName;

        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public ClassDefTable GlyphClassDef { get; set; }
        public AttachmentListTable AttachmentListTable { get; set; }
        public LigCaretList LigCaretList { get; set; }
        public ClassDefTable MarkAttachmentClassDef { get; set; }
        public MarkGlyphSetsTable MarkGlyphSetsTable { get; set; }
    }
}
