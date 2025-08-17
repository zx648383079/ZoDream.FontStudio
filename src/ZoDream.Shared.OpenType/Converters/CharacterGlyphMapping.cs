using System;
using System.Collections.Generic;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    /// <summary>
    /// https://www.microsoft.com/typography/otspec/cmap.htm
    /// </summary>
    public class CharacterGlyphMappingConverter : TypefaceConverter<CharacterGlyphMappingTable>
    {
        public override CharacterGlyphMappingTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new CharacterGlyphMappingTable();
            var beginAt = reader.Position;
            var version = reader.ReadUInt16(); // 0
            var tableCount = reader.ReadUInt16();

            var platformIds = new ushort[tableCount];
            var encodingIds = new ushort[tableCount];
            var offsets = new uint[tableCount];
            for (int i = 0; i < tableCount; i++)
            {
                platformIds[i] = reader.ReadUInt16();
                encodingIds[i] = reader.ReadUInt16();
                offsets[i] = reader.ReadUInt32();
            }

            res.CharacterMaps = new CharacterMap[tableCount];
            for (int i = 0; i < tableCount; i++)
            {
                reader.BaseStream.Seek(beginAt + offsets[i], SeekOrigin.Begin);
                var cmap = ReadCharacterMap(reader);
                cmap.PlatformId = platformIds[i];
                cmap.EncodingId = encodingIds[i];
                res.CharacterMaps[i] = cmap;

            }
            return res;
        }

        private CharacterMap ReadCharacterMap(EndianReader reader)
        {
            var format = reader.ReadUInt16();
            return format switch
            {
                0 => ReadFormat0(reader),
                2 => ReadFormat2(reader),
                4 => ReadFormat4(reader),
                6 => ReadFormat6(reader),
                12 => ReadFormat12(reader),
                14 => ReadFormat14(reader),
                _ => new NullCharMap(),
            };
        }

        private CharacterMap ReadFormat14(EndianReader reader)
        {
            long beginAt = reader.BaseStream.Position - 2; // account for header format entry 
            uint length = reader.ReadUInt32(); // Byte length of this subtable (including the header)
            uint numVarSelectorRecords = reader.ReadUInt32();

            var variationSelectors = new Dictionary<int, VariationSelector>();
            int[] varSelectors = new int[numVarSelectorRecords];
            uint[] defaultUVSOffsets = new uint[numVarSelectorRecords];
            uint[] nonDefaultUVSOffsets = new uint[numVarSelectorRecords];
            for (int i = 0; i < numVarSelectorRecords; ++i)
            {
                varSelectors[i] = reader.ReadUInt24();
                defaultUVSOffsets[i] = reader.ReadUInt32();
                nonDefaultUVSOffsets[i] = reader.ReadUInt32();
            }
            for (int i = 0; i < numVarSelectorRecords; ++i)
            {
                var sel = new VariationSelector();
                if (defaultUVSOffsets[i] != 0)
                {
                    reader.BaseStream.Seek(beginAt + defaultUVSOffsets[i], SeekOrigin.Begin);
                    uint numUnicodeValueRanges = reader.ReadUInt32();
                    for (int n = 0; n < numUnicodeValueRanges; ++n)
                    {
                        var startCode = (int)reader.ReadUInt24();
                        sel.DefaultStartCodes.Add(startCode);
                        sel.DefaultEndCodes.Add(startCode + reader.ReadByte());
                    }
                }

                if (nonDefaultUVSOffsets[i] != 0)
                {
                    reader.BaseStream.Seek(beginAt + nonDefaultUVSOffsets[i], SeekOrigin.Begin);
                    uint numUVSMappings = reader.ReadUInt32();
                    for (int n = 0; n < numUVSMappings; ++n)
                    {
                        int unicodeValue = (int)reader.ReadUInt24();
                        ushort glyphID = reader.ReadUInt16();
                        sel.UVSMappings.Add(unicodeValue, glyphID);
                    }
                }

                variationSelectors.Add(varSelectors[i], sel);
            }
            return new CharMapFormat14 { VariationSelectors = variationSelectors };
        }

        private CharacterMap ReadFormat12(EndianReader reader)
        {
            ushort reserved = reader.ReadUInt16();
            uint length = reader.ReadUInt32();
            uint language = reader.ReadUInt32();
            uint numGroups = reader.ReadUInt32();
            uint[] startCharCodes = new uint[numGroups];
            uint[] endCharCodes = new uint[numGroups];
            uint[] startGlyphIds = new uint[numGroups];


            for (uint i = 0; i < numGroups; ++i)
            {
                //seq map group record
                startCharCodes[i] = reader.ReadUInt32();
                endCharCodes[i] = reader.ReadUInt32();
                startGlyphIds[i] = reader.ReadUInt32();
            }
            return new CharMapFormat12(startCharCodes, endCharCodes, startGlyphIds);
        }

        private CharacterMap ReadFormat6(EndianReader reader)
        {
            ushort length = reader.ReadUInt16();
            ushort language = reader.ReadUInt16();
            ushort firstCode = reader.ReadUInt16();
            ushort entryCount = reader.ReadUInt16();
            ushort[] glyphIdArray = reader.ReadArray(entryCount, reader.ReadUInt16);
            return new CharMapFormat6(firstCode, glyphIdArray);
        }

        private CharacterMap ReadFormat4(EndianReader reader)
        {
            ushort lenOfSubTable = reader.ReadUInt16();
            long tableStartEndAt = reader.Position + lenOfSubTable;

            ushort language = reader.ReadUInt16();
            ushort segCountX2 = reader.ReadUInt16();
            ushort searchRange = reader.ReadUInt16();
            ushort entrySelector = reader.ReadUInt16();
            ushort rangeShift = reader.ReadUInt16();
            int segCount = segCountX2 / 2;
            ushort[] endCode = reader.ReadArray(segCount, reader.ReadUInt16);

            ushort Reserved = reader.ReadUInt16(); // always 0
            ushort[] startCode = reader.ReadArray(segCount, reader.ReadUInt16); ; //Starting character code for each segment
            ushort[] idDelta = reader.ReadArray(segCount, reader.ReadUInt16); ; //Delta for all character codes in segment
            ushort[] idRangeOffset = reader.ReadArray(segCount, reader.ReadUInt16); ; //Offset in bytes to glyph indexArray, or 0   
                                                                             //------------------------------------------------------------------------------------ 
            long remainingLen = tableStartEndAt - reader.Position;
            int recordNum2 = (int)(remainingLen / 2);
            ushort[] glyphIdArray = reader.ReadArray(recordNum2, reader.ReadUInt16);//Glyph index array                          
            return new CharMapFormat4(startCode, endCode, idDelta, idRangeOffset, glyphIdArray);
        }

        private CharacterMap ReadFormat2(EndianReader reader)
        {
            return new NullCharMap();
        }

        private CharacterMap ReadFormat0(EndianReader reader)
        {
            ushort length = reader.ReadUInt16();
            ushort language = reader.ReadUInt16();
            byte[] only256Glyphs = reader.ReadBytes(256);
            ushort[] only256UInt16Glyphs = new ushort[256];
            for (int i = 255; i >= 0; i --)
            {
                //expand
                only256UInt16Glyphs[i] = only256Glyphs[i];
            }
            //convert to format4 cmap table
            var startArray = new ushort[] { 0, 0xFFFF };
            var endArray = new ushort[] { 255, 0xFFFF };
            var deltaArray = new ushort[] { 0, 1 };
            var offsetArray = new ushort[] { 4, 0 };
            return new CharMapFormat4(startArray, endArray, deltaArray, offsetArray, only256UInt16Glyphs);
        }

        public override void Write(EndianWriter writer, CharacterGlyphMappingTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
