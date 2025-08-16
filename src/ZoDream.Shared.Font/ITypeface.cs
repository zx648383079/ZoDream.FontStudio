using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ZoDream.Shared.Font
{
    public interface ITypeface
    {

        public bool Contains(char character);

        public void TryGet(char character, [NotNullWhen(true)] Glyph? result);
    }

    public interface ITypefaceCollection : ICollection<ITypeface>, IEnumerable<ITypeface>
    {
    }
}
