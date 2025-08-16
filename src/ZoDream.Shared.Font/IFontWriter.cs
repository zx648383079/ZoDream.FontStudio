using System;
using System.IO;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceWriter : IDisposable
    {
        public void Write(Stream output);
    }
}
