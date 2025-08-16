using System;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceReader : IDisposable
    {

        public ITypefaceCollection Read();
    }


}
