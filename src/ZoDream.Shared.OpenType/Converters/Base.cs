using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class BaseConverter : TypefaceConverter<BaseTable>
    {
        public override BaseTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new BaseTable();

            long tableStartAt = reader.BaseStream.Position;

            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            ushort horizAxisOffset = reader.ReadUInt16();
            ushort vertAxisOffset = reader.ReadUInt16();
            uint itemVarStoreOffset = 0;
            if (minorVersion == 1)
            {
                itemVarStoreOffset = reader.ReadUInt32();
            }

            //Axis Tables: HorizAxis and VertAxis 

            if (horizAxisOffset > 0)
            {
                reader.BaseStream.Position = tableStartAt + horizAxisOffset;
                res.HorizontalAxis = ReadAxisTable(reader);
                res.HorizontalAxis.isVerticalAxis = false;
            }
            if (vertAxisOffset > 0)
            {
                reader.BaseStream.Position = tableStartAt + vertAxisOffset;
                res.VerticalAxis = ReadAxisTable(reader);
                res.VerticalAxis.isVerticalAxis = true;
            }
            if (itemVarStoreOffset > 0)
            {
                //TODO
            }
            return res;
        }

        private AxisTable ReadAxisTable(EndianReader reader)
        {

            long axisTableStartAt = reader.BaseStream.Position;

            ushort baseTagListOffset = reader.ReadUInt16();
            ushort baseScriptListOffset = reader.ReadUInt16();

            var axisTable = new AxisTable();
            if (baseTagListOffset > 0)
            {
                reader.BaseStream.Position = axisTableStartAt + baseTagListOffset;
                axisTable.baseTagList = reader.ReadArray(reader.ReadUInt16(), () => reader.ReadString(4));
            }
            if (baseScriptListOffset > 0)
            {
                reader.BaseStream.Position = axisTableStartAt + baseScriptListOffset;
                axisTable.baseScripts = ReadBaseScriptList(reader);
            }
            return axisTable;
        }

        private BaseScript[] ReadBaseScriptList(EndianReader reader)
        {
            long baseScriptListStartAt = reader.BaseStream.Position;
            ushort baseScriptCount = reader.ReadUInt16();

            var baseScriptRecord_offsets = reader.ReadRecord(baseScriptCount);
            BaseScript[] baseScripts = new BaseScript[baseScriptCount];
            for (int i = 0; i < baseScriptCount; ++i)
            {
                var baseScriptRecord = baseScriptRecord_offsets[i];
                reader.BaseStream.Position = baseScriptListStartAt + baseScriptRecord.Offset;
                //
                BaseScript baseScipt = ReadBaseScriptTable(reader);
                baseScipt.ScriptIdenTag = baseScriptRecord.Tag;
                baseScripts[i] = baseScipt;
            }
            return baseScripts;
        }

        private BaseScript ReadBaseScriptTable(EndianReader reader)
        {
            long baseScriptTableStartAt = reader.BaseStream.Position;
            ushort baseValueOffset = reader.ReadUInt16();
            ushort defaultMinMaxOffset = reader.ReadUInt16();
            ushort baseLangSysCount = reader.ReadUInt16();
            RecordEntry[] baseLangSysRecords = null;

            if (baseLangSysCount > 0)
            {
                baseLangSysRecords = reader.ReadRecord(baseLangSysCount);
            }

            BaseScript baseScript = new BaseScript();
            baseScript.baseLangSysRecords = baseLangSysRecords;
            //--------------------
            if (baseValueOffset > 0)
            {
                reader.BaseStream.Position = baseScriptTableStartAt + baseValueOffset;
                baseScript.baseValues = ReadBaseValues(reader);

            }
            if (defaultMinMaxOffset > 0)
            {
                reader.BaseStream.Position = baseScriptTableStartAt + defaultMinMaxOffset;
                baseScript.MinMax = ReadMinMaxTable(reader);
            }

            return baseScript;
        }

        private MinMax ReadMinMaxTable(EndianReader reader)
        {
            long startMinMaxTableAt = reader.BaseStream.Position;
            //
            MinMax minMax = new MinMax();
            ushort minCoordOffset = reader.ReadUInt16();
            ushort maxCoordOffset = reader.ReadUInt16();
            ushort featMinMaxCount = reader.ReadUInt16();

            FeatureMinMaxOffset[] minMaxFeatureOffsets = null;
            if (featMinMaxCount > 0)
            {
                minMaxFeatureOffsets = new FeatureMinMaxOffset[featMinMaxCount];
                for (int i = 0; i < featMinMaxCount; ++i)
                {
                    minMaxFeatureOffsets[i] = new FeatureMinMaxOffset(
                        reader.ReadString(4), //featureTableTag
                        reader.ReadUInt16(), //minCoord offset
                        reader.ReadUInt16() //maxCoord offset
                        );
                }
            }

            //----------
            if (minCoordOffset > 0)
            {
                minMax.minCoord = ReadBaseCoordTable(reader, startMinMaxTableAt + minCoordOffset);
            }
            if (maxCoordOffset > 0)
            {
                minMax.maxCoord = ReadBaseCoordTable(reader, startMinMaxTableAt + maxCoordOffset);
            }

            if (minMaxFeatureOffsets != null)
            {
                var featureMinMaxRecords = new FeatureMinMax[minMaxFeatureOffsets.Length];
                for (int i = 0; i < minMaxFeatureOffsets.Length; ++i)
                {
                    FeatureMinMaxOffset featureMinMaxOffset = minMaxFeatureOffsets[i];

                    featureMinMaxRecords[i] = new FeatureMinMax(
                        featureMinMaxOffset.featureTableTag, //tag
                        ReadBaseCoordTable(reader, startMinMaxTableAt + featureMinMaxOffset.minCoord), //min
                        ReadBaseCoordTable(reader, startMinMaxTableAt + featureMinMaxOffset.maxCoord)); //max
                }
                minMax.featureMinMaxRecords = featureMinMaxRecords;
            }

            return minMax;
        }

        private BaseValues ReadBaseValues(EndianReader reader)
        {
            long baseValueTableStartAt = reader.BaseStream.Position;

            //
            ushort defaultBaselineIndex = reader.ReadUInt16();
            ushort baseCoordCount = reader.ReadUInt16();
            ushort[] baseCoords_Offset = reader.ReadArray(baseCoordCount, reader.ReadUInt16);

            BaseCoord[] baseCoords = new BaseCoord[baseCoordCount];
            for (int i = 0; i < baseCoordCount; ++i)
            {
                baseCoords[i] = ReadBaseCoordTable(reader, baseValueTableStartAt + baseCoords_Offset[i]);
            }

            return new BaseValues(defaultBaselineIndex, baseCoords);
        }

        private BaseCoord ReadBaseCoordTable(EndianReader reader, long pos)
        {
            reader.BaseStream.Position = pos;
            ushort baseCoordFormat = reader.ReadUInt16();
            return baseCoordFormat switch
            {
                1 => new BaseCoord(1,
                                        reader.ReadInt16()),//coord
                2 => new BaseCoord(2,
                                        reader.ReadInt16(), //coordinate
                                        reader.ReadUInt16(), //referenceGlyph
                                        reader.ReadUInt16()),//baseCoordPoint
                3 => new BaseCoord(),
                _ => throw new System.NotSupportedException(),
            };
        }

        public override void Write(EndianWriter writer, BaseTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
