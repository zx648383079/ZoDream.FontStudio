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
            var res = new EmbeddedBitmapScalingTable();
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            var numSizes = reader.ReadUInt32();
            res.Strikes = reader.ReadArray((int)numSizes, () => ReadBitmapScale(reader));
            return res;
        }

        private BitmapScale ReadBitmapScale(EndianReader reader)
        {
            var res = new BitmapScale();
            res.Hori = MetricsConverter.ReadSbitLine(reader);
            res.Vert = MetricsConverter.ReadSbitLine(reader);

            res.PpemX = reader.ReadByte();
            res.PpemY = reader.ReadByte();
            res.SubstitutePpemX = reader.ReadByte();
            res.SubstitutePpemY = reader.ReadByte();
            return res;
        }

        public override void Write(EndianWriter writer, EmbeddedBitmapScalingTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
