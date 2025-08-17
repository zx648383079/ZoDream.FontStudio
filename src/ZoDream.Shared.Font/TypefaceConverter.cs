using System;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.Font
{
    public abstract class TypefaceConverter : ITypefaceConverter
    {
        public abstract bool CanConvert(Type objectType);

        public abstract object? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer);

        public abstract void Write(EndianWriter writer, object data, Type objectType, ITypefaceSerializer serializer);
    }

    public abstract class TypefaceConverter<T> : ITypefaceConverter<T>
    {
        public bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public abstract T? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer);

        public void Write(EndianWriter writer, object data, Type objectType, ITypefaceSerializer serializer)
        {
            Write(writer, (T)data, objectType, serializer);
        }

        public abstract void Write(EndianWriter writer, T data, Type objectType, ITypefaceSerializer serializer);

        object? ITypefaceConverter.Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return Read(reader, objectType, serializer);
        }
    }

    public abstract class TypefaceTableConverter<T> : ITypefaceTableConverter<T>
        where T : ITypefaceTable
    {
        public bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public abstract T? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer);

        public virtual object? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public abstract void Write(EndianWriter writer, T data);

        public void Write(EndianWriter writer, object data, Type objectType, ITypefaceSerializer serializer)
        {
            Write(writer, (T)data);
        }
    }
}
