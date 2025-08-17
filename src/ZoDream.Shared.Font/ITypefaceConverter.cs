using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceConverterCollection : ICollection<ITypefaceConverter>
    {
        public bool TryGet<T>([NotNullWhen(true)] out ITypefaceConverter? converter);
        public bool TryGet(Type objectType, [NotNullWhen(true)] out ITypefaceConverter? converter);
    }

    public interface ITypefaceConverter
    {
        public bool CanConvert(Type objectType);

        public object? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer);
        public void Write(EndianWriter writer, object data, Type objectType, ITypefaceSerializer serializer);
    }

    public interface ITypefaceConverter<T> : ITypefaceConverter
    {
        public new T? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer);
        public void Write(EndianWriter writer, T data, Type objectType, ITypefaceSerializer serializer);
    }



    public interface ITypefaceSerializer
    {
        /// <summary>
        /// 默认倒叙查找，所以 子类转换器请放在父类转换器之后
        /// </summary>
        public ITypefaceConverterCollection Converters { get; }

        public T? Deserialize<T>(EndianReader reader);
        public object? Deserialize(EndianReader reader, Type objectType);

        public void Serialize<T>(EndianWriter writer, T data);
        public void Serialize(EndianWriter writer, object data, Type objectType);
    }


}
