using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class ColorConverter : TypefaceConverter<ColorTable>
    {
        public override ColorTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new ColorTable();
            long offset = reader.BaseStream.Position;
            ushort version = reader.ReadUInt16();
            ushort numBaseGlyphRecords = reader.ReadUInt16();
            uint baseGlyphRecordsOffset = reader.ReadUInt32();
            uint layerRecordsOffset = reader.ReadUInt32();
            ushort numLayerRecords = reader.ReadUInt16();

            res.GlyphLayers = new ushort[numLayerRecords];
            res.GlyphPalettes = new ushort[numLayerRecords];

            reader.BaseStream.Seek(offset + baseGlyphRecordsOffset, SeekOrigin.Begin);
            for (int i = 0; i < numBaseGlyphRecords; ++i)
            {
                ushort gid = reader.ReadUInt16();
                res.LayerIndices[gid] = reader.ReadUInt16();
                res.LayerCounts[gid] = reader.ReadUInt16();
            }

            reader.BaseStream.Seek(offset + layerRecordsOffset, SeekOrigin.Begin);
            for (int i = 0; i < res.GlyphLayers.Length; ++i)
            {
                res.GlyphLayers[i] = reader.ReadUInt16();
                res.GlyphPalettes[i] = reader.ReadUInt16();
            }
            return res;
        }

        public override void Write(EndianWriter writer, ColorTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
