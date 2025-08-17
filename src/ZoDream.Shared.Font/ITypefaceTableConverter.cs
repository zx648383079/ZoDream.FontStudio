using System.Diagnostics.CodeAnalysis;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceTableConverter<T> : ITypefaceConverter
        where T : ITypefaceTable
    {
        public T? Read(EndianReader reader,
            ITypefaceTableEntry entry,
            ITypefaceTableSerializer serializer);
        public void Write(EndianWriter writer, T data);
    }

    public interface ITypefaceTableSerializer
    {
        public bool TryGet<T>([NotNullWhen(true)] out T? result) where T : ITypefaceTable;
    }
}
