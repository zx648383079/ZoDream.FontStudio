using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphPositioningTable : GlyphShapingTable
    {
        public const string TableName = "GPOS";

        public override string Name => TableName;

        public IList<LookupTable> LookupList { get; internal set; } = [];

        public bool EnableLongLookBack { get; internal set; }
   
    }
}
