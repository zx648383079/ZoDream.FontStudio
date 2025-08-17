using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MaxProfileConverter : TypefaceConverter<MaxProfileTable>
    {
        public override MaxProfileTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new MaxProfileTable();
            res.Version = reader.ReadUInt32(); // 0x00010000 == 1.0
            res.GlyphCount = reader.ReadUInt16();
            res.MaxPointsPerGlyph = reader.ReadUInt16();
            res.MaxContoursPerGlyph = reader.ReadUInt16();
            res.MaxPointsPerCompositeGlyph = reader.ReadUInt16();
            res.MaxContoursPerCompositeGlyph = reader.ReadUInt16();
            res.MaxZones = reader.ReadUInt16();
            res.MaxTwilightPoints = reader.ReadUInt16();
            res.MaxStorage = reader.ReadUInt16();
            res.MaxFunctionDefs = reader.ReadUInt16();
            res.MaxInstructionDefs = reader.ReadUInt16();
            res.MaxStackElements = reader.ReadUInt16();
            res.MaxSizeOfInstructions = reader.ReadUInt16();
            res.MaxComponentElements = reader.ReadUInt16();
            res.MaxComponentDepth = reader.ReadUInt16();
            return res;
        }

        public override void Write(EndianWriter writer, MaxProfileTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
