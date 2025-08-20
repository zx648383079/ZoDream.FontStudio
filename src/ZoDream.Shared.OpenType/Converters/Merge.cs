using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MergeConverter : TypefaceConverter<MergeTable>
    {
        public override MergeTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var beginAt = reader.Position;
            var res = new MergeTable();
            var version = reader.ReadUInt16();
            var mergeClassCount = reader.ReadUInt16();
            var mergeDataOffset = reader.ReadUInt16();
            var classDefCount = reader.ReadUInt16();
            var offsetToClassDefOffsets = reader.ReadUInt16();

            reader.Position = beginAt + mergeDataOffset;
            res.MergeData = new()
            {
                MergeEntryRows = reader.ReadArray(mergeClassCount, () => {
                    return new MergeEntryRow()
                    {
                        MergeEntries = reader.ReadArray(mergeClassCount, reader.ReadByte)
                    };
                })
            };
            reader.Position = beginAt + offsetToClassDefOffsets;
            var classDefOffsets = reader.ReadUInt16Array(classDefCount);
            res.ClassDefs = reader.ReadArray(classDefOffsets, offset => GlyphDefinitionConverter.ReadClassDefTable(reader, beginAt + offset));
            return res;
        }

        public override void Write(EndianWriter writer, MergeTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
