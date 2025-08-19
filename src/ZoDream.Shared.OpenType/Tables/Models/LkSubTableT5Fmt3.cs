using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.Tables.Models
{
    internal class LkSubTableT5Fmt3 : LookupSubTable
    {
        public override void CollectAssociatedSubtitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            throw new NotImplementedException();
        }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            throw new NotImplementedException();
        }
    }
}
