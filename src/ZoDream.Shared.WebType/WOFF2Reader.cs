using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType;
using ZoDream.Shared.OpenType.Converters;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.WebType
{
    public partial class WOFF2Reader(EndianReader reader) : ITypefaceReader
    {
        public static byte[] Signature = "wOF2"u8.ToArray();

        public static TypefaceConverterCollection Converters = [
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
            new Converters.GlyphDataConverter(),
            new Converters.GlyphLocationsConverter(),
            new GlyphDefinitionConverter(),
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

        public WOFF2Reader(Stream input) : this(new EndianReader(input, EndianType.BigEndian, false))
        {

        }

        public ITypefaceCollection Read()
        {
            var buffer = reader.ReadBytes(Signature.Length);
            Debug.Assert(buffer.SequenceEqual(Signature));
            var res = new Typeface();
            var header = ReadHeader();
            var entries = ReadEntry(header).ToArray();
            using var ms = new MemoryStream();
            new BrotliStream(new PartialStream(reader.BaseStream, header.TotalCompressedSize), 
                CompressionMode.Decompress).CopyTo(ms);
            ms.Position = 0;
            var data = entries.ToCollection();
            var serializer = new TypefaceTableSerializer(ms, new TypefaceSerializer(OTFReader.Converters), data);
            foreach (var item in entries)
            {
                serializer.TryGet(item, out _);
            }
            return new TypefaceCollection
            {
                res
            };
        }

        private IEnumerable<WOFFTableEntry> ReadEntry(WOFFFileHeader header)
        {
            var expectedStartAt = 0L;
            for (int i = 0; i < header.NumTables; i++)
            {
                var flags = reader.ReadByte();
                var knowTable = flags & 0x1F;
                var entry = new WOFFTableEntry()
                {
                    Name = (knowTable < 63) ? _knownTableTags[knowTable] : reader.ReadString(4),
                    PreprocessingTransformation = (byte)((flags >> 5) & 0x3),
                    Offset = expectedStartAt,
                    Length = reader.Read7BitEncodedUInt(),
                };
                if (entry.PreprocessingTransformation == 0 && entry.Name is GlyphDataTable.TableName or GlyphLocationsTable.TableName)
                {
                    entry.OriginalLength = entry.Length;
                    // 预处理长度和原始长度
                    entry.Length = reader.Read7BitEncodedUInt();
                }
                yield return entry;
                expectedStartAt += entry.Length;
            }
        }

        private WOFFFileHeader ReadHeader()
        {
            var header = new WOFFFileHeader
            {
                Flavor = reader.ReadUInt32(),
                Length = reader.ReadUInt32(),
                NumTables = reader.ReadUInt16(),
                Reserved = reader.ReadUInt16(),
                TotalSfntSize = reader.ReadUInt32(),
                TotalCompressedSize = reader.ReadUInt32(),

                MajorVersion = reader.ReadUInt16(),
                MinorVersion = reader.ReadUInt16(),

                MetaOffset = reader.ReadUInt32(),
                MetaLength = reader.ReadUInt32(),
                MetaOriginalLength = reader.ReadUInt32(),

                PrivOffset = reader.ReadUInt32(),
                PrivLength = reader.ReadUInt32()
            };
            return header;
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
