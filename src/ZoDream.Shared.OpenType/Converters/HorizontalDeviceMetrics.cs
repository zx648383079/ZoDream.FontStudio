using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class HorizontalDeviceMetricsConverter : TypefaceConverter<HorizontalDeviceMetricsTable>
    {
        public override HorizontalDeviceMetricsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return new HorizontalDeviceMetricsTable();
        }

        public override void Write(EndianWriter writer, HorizontalDeviceMetricsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
