using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MetricsConverter : TypefaceConverter<SmallGlyphMetrics>
    {
        public override SmallGlyphMetrics Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            return ReadSmall(reader);
        }

        public static SmallGlyphMetrics ReadSmall(EndianReader reader)
        {
            return new SmallGlyphMetrics
            {
                Height = reader.ReadByte(),
                Width = reader.ReadByte(),

                BearingX = reader.ReadSByte(),
                BearingY = reader.ReadSByte(),
                Advance = reader.ReadByte()
            };
        }

        public static BigGlyphMetrics ReadBig(EndianReader reader)
        {
            return new BigGlyphMetrics
            {
                Height = reader.ReadByte(),
                Width = reader.ReadByte(),

                HorizontalBearingX = reader.ReadSByte(),
                HorizontalBearingY = reader.ReadSByte(),
                HorizontalAdvance = reader.ReadByte(),

                VerticalBearingX = reader.ReadSByte(),
                VerticalBearingY = reader.ReadSByte(),
                VerticalAdvance = reader.ReadByte(),
            };
        }

        public static SbitLineMetrics ReadSbitLine(EndianReader reader)
        {
            return new SbitLineMetrics()
            {
                Ascender = reader.ReadSByte(),
                Descender = reader.ReadSByte(),
                WidthMax = reader.ReadByte(),
                CaretSlopeNumerator = reader.ReadSByte(),
                CaretSlopeDenominator = reader.ReadSByte(),
                CaretOffset = reader.ReadSByte(),
                MinOriginSB = reader.ReadSByte(),
                MinAdvanceSB = reader.ReadSByte(),
                MaxBeforeBL = reader.ReadSByte(),
                MinAfterBL = reader.ReadSByte(),
                Pad1 = reader.ReadSByte(),
                Pad2 = reader.ReadSByte()
            };
        }

        public override void Write(EndianWriter writer, SmallGlyphMetrics data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
