using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class CVTVariationsConverter : TypefaceConverter<CVTVariationsTable>
    {
        public override CVTVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new CVTVariationsTable();
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            var tupleVariationCount = reader.ReadUInt16();
            var dataOffset = reader.ReadUInt16();

            res.TupleVariationHeaders = reader.ReadArray(tupleVariationCount, () => {
                return GlyphVariationsConverter.ReadTupleVariationHeader(reader, 0);
            });
            return res;
        }

        public override void Write(EndianWriter writer, CVTVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
