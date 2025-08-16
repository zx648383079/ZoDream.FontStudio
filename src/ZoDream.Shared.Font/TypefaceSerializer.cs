using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.Font
{
    public class TypefaceConverterCollection : Collection<ITypefaceConverter>, ITypefaceConverterCollection
    {
        public TypefaceConverterCollection()
        {

        }

        public TypefaceConverterCollection(IList<ITypefaceConverter> items)
            : base(items)
        {

        }

        public bool TryGet<T>([NotNullWhen(true)] out ITypefaceConverter? converter)
        {
            return TryGet(typeof(T), out converter);
        }

        public bool TryGet(Type objectType, [NotNullWhen(true)] out ITypefaceConverter? converter)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (this[i].CanConvert(objectType))
                {
                    converter = this[i];
                    return true;
                }
            }
            converter = null;
            return false;
        }
    }

    public class TypefaceSerializer : ITypefaceSerializer
    {
        public TypefaceSerializer()
        {
            Converters = new TypefaceConverterCollection();
        }
        public TypefaceSerializer(IEnumerable<ITypefaceConverter> items)
        {
            Converters = items is ITypefaceConverterCollection o ? o : new TypefaceConverterCollection([.. items]);
        }
        public ITypefaceConverterCollection Converters { get; private set; }

        public T? Deserialize<T>(EndianReader reader)
        {
            return (T?)Deserialize(reader, typeof(T));
        }

        public object? Deserialize(EndianReader reader, Type objectType)
        {
            if (Converters.TryGet(objectType, out var converter))
            {
                return converter.Read(reader, objectType, this);
            }
            return null;
        }

        public void Serialize<T>(EndianWriter writer, T data)
        {
            Serialize(writer, data, typeof(T));
        }

        public void Serialize(EndianWriter writer, object data, Type objectType)
        {
            if (Converters.TryGet(objectType, out var converter))
            {
                converter.Write(writer, data, objectType, this);
            }
        }
    }
}
