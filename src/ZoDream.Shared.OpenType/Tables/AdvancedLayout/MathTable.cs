using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class MathTable : ITypefaceTable
    {
        public const string TableName = "MATH";

        public string Name => TableName;

        public MathConstants ConstTable { get; internal set; }

        public MathVariantsTable VariantsTable { get; internal set; }
        public CoverageTable ExtendedShapeCoverageTable { get; internal set; }

        public CoverageTable KernInfoCoverage { get; internal set; }

        public MathKernInfoRecord[] KernInfoRecords {  get; internal set; }

        internal MathTopAccentAttachmentTable TopAccentAttachmentTable { get; set; }
        internal MathItalicsCorrectonInfoTable ItalicCorrectionInfo { get; set; }
    }



    
}
