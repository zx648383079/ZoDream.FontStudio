using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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

        public bool TryGet(ITypefaceTableEntry entry, [NotNullWhen(true)] out ITypefaceTable? result)
        {
            if (!serializer.Converters.TryGet(entry.Name, out var targetType))
            {
                result = null;
                return false;
            }
            if (TryGet(targetType, out var res)) 
            {
                result = (ITypefaceTable)res;
                return true;
            }
            result = null;
            return false;
        }

        public bool TryGet(Type targetType, [NotNullWhen(true)] out object? result)
        {
            var property = targetType.GetProperty("TableName", BindingFlags.Static | BindingFlags.Public);
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
                result = res;
                return true;
            }
            if (!serializer.Converters.TryGet(targetType, out var cvt))
            {
                result = default;
                return false;
            }
            var reader = new EndianReader(
                new PartialStream(input, u.Entry.Offset, u.Entry.Length),
                EndianType.BigEndian);
            var interfaceType = cvt.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITypefaceTableConverter<>));
            if (interfaceType is not null)
            {
                var fn = interfaceType.GetMethod("Read", [
                    typeof(EndianReader),
                    typeof(ITypefaceTableEntry),
                    typeof(ITypefaceTableSerializer)]);
                result = fn?.Invoke(cvt, [reader, u.Entry, this]);
            }
            else
            {
                result = cvt.Read(reader, targetType, serializer);
            }
            if (result is null)
            {
                return false;
            }
            tables.Add((ITypefaceTable)result);
            return true;
        }

        public bool TryGet<T>([NotNullWhen(true)] out T? result) where T : ITypefaceTable
        {
            if (TryGet(typeof(T), out var res))
            {
                result = (T)res;
                return true;
            }
            result = default;
            return false;
        }

    }
}
