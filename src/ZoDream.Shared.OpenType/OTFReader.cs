using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Converters;

namespace ZoDream.Shared.OpenType
{
    public class OTFReader(EndianReader reader) : TTFReader(reader)
    {

        public static ITypefaceConverter[] Converters = [
            new AxisVariationsConverter(),
            new BaseConverter(),
            new BitmapSizeConverter(),
            new CharacterGlyphMappingConverter(),
            new ColorConverter(),
            new ColorBitmapDataConverter(),
            new ColorBitmapLocationConverter(),
            new ColorPaletteConverter(),
            new CompactFontFormatConverter(),
            new CompactFontFormat2Converter(),
            new ControlValueConverter(),
            new ControlValueProgramConverter(),
            new CoverageConverter(),
            new CVTVariationsConverter(),
            new DigitalSignatureConverter(),
            new EmbeddedBitmapDataConverter(),
            new EmbeddedBitmapLocationConverter(),
            new EmbeddedBitmapScalingConverter(),
            new FontProgramConverter(),
            new FontVariationsConverter(),
            new GlyphDataConverter(),
            new GlyphDefinitionConverter(),
            new GlyphLocationsConverter(),
            new GlyphPositioningConverter(),
            new GlyphSubstitutionConverter(),
            new GlyphVariationsConverter(),
            new GridFittingScanConversionProcedureConverter(),
            new HeadConverter(),
            new HorizontalDeviceMetricsConverter(),
            new HorizontalHeaderConverter(),
            new HorizontalMetricsConverter(),
            new HorizontalMetricsVariationsConverter(),
            new IndexSubConverter(),
            new JustificationConverter(),
            new KernConverter(),
            new LinearThresholdConverter(),
            new MathConverter(),
            new MaxProfileConverter(),
            new MergeConverter(),
            new MetaConverter(),
            new MetricsConverter(),
            new MetricsVariationsConverter(),
            new NameConverter(),
            new OS2Converter(),
            new Pcl5Converter(),
            new PostConverter(),
            new StandardBitmapGraphicsConverter(),
            new StyleAttributesConverter(),
            new SvgConverter(),
            new VerticalDeviceMetricsConverter(),
            new VerticalHeaderConverter(),
            new VerticalMetricsConverter(),
            new VerticalMetricsVariationsConverter(),
            new VerticalOriginConverter(),
        ];

        public OTFReader(Stream input) : this(new EndianReader(input, EndianType.BigEndian, false))
        {

        }
    }
}
