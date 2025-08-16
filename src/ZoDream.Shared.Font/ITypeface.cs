using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Font
{
    public interface ITypeface
    {

        public bool Contains(char character);

        public void TryGet(char character, [NotNullWhen(true)] Glyph? result);
    }

    public interface ITypefaceCollection : IEnumerable<ITypeface>
    {
        public int Count { get; }
    }
}
