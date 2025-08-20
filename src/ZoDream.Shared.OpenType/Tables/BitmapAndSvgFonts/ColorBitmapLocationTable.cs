using System.Collections.Generic;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ColorBitmapLocationTable : ITypefaceTable
    {
        public const string TableName = "CBLC";

        public string Name => TableName;

        public BitmapSizeTable[] BmpSizeTables { get; internal set; }


        public GlyphData[] BuildGlyphList()
        {
            var glyphs = new List<GlyphData>();
            int numSizes = BmpSizeTables.Length;
            for (int n = 0; n < numSizes; ++n)
            {
                var bmpSizeTable = BmpSizeTables[n];
                uint numberOfIndexSubTables = bmpSizeTable.NumberOfIndexSubTables;
                for (uint i = 0; i < numberOfIndexSubTables; ++i)
                {
                    bmpSizeTable.IndexSubTables[i].BuildGlyphList(glyphs);
                }
            }
            return [.. glyphs];
        }
    }
}
