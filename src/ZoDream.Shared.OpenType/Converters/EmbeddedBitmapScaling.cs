using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class EmbeddedBitmapScalingConverter : TypefaceConverter<EmbeddedBitmapScalingTable>
    {
        public override EmbeddedBitmapScalingTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return new EmbeddedBitmapScalingTable();
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapScalingTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
