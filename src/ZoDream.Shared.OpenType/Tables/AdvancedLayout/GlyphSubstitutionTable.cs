using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphSubstitutionTable : GlyphShapingTable
    {
        public const string TableName = "GSUB";

        public override string Name => TableName;

        public IList<LookupTable> LookupList { get; set; } = [];
    }
}
