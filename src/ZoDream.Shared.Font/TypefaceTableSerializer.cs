using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.Font
{
    public class TypefaceTableSerializer(
        Stream input,
        ITypefaceSerializer serializer,
        ITypefaceTableCollection tables
        ) : ITypefaceTableSerializer
    {
        public bool TryGet<T>([NotNullWhen(true)] out T? result) where T : ITypefaceTable
        {
            var type = typeof(T);
            var property = type.GetProperty("TableName", BindingFlags.Static | BindingFlags.Public);
            var name = property?.GetValue(null)?.ToString();
            if (property is null || string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException($"<T> doesn't have 'TableName' Property");
            }
            if (!tables.TryGet(name, out var res))
            {
                result = default;
                return false;
            }
            if (res is not IUnreadTypefaceTable u)
            {
                result = (T)res;
                return true;
            }
            if (!serializer.Converters.TryGet(type, out var cvt))
            {
                result = default;
                return false;
            }
            var reader = new EndianReader(
                new PartialStream(input, u.Entry.Offset, u.Entry.Length), 
                EndianType.BigEndian);
            if (cvt is ITypefaceTableConverter<T> tc)
            {
                result = tc.Read(reader, u.Entry, this);
            } 
            else
            {
                result = (T)cvt.Read(reader, typeof(T), serializer);
            }
            if (result is null)
            {
                return false;
            }
            tables.Add(result);
            return true;
        }

    }
}
