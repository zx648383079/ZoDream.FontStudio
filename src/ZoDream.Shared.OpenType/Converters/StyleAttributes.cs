using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class StyleAttributesConverter : TypefaceConverter<StyleAttributesTable>
    {
        public override StyleAttributesTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new StyleAttributesTable();
            long beginPos = reader.BaseStream.Position;
            //
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            ushort designAxisSize = reader.ReadUInt16();
            ushort designAxisCount = reader.ReadUInt16();
            //
            uint designAxesOffset = reader.ReadUInt32();
            ushort axisValueCount = reader.ReadUInt16();
            uint offsetToAxisValueOffsets = reader.ReadUInt32();
            //
            ushort elidedFallbackNameID = (minorVersion != 0) ? reader.ReadUInt16() : (ushort)0;

            var axisRecords = new AxisRecord[designAxisCount];
            for (int i = 0; i < designAxisCount; ++i)
            {
                var axisRecord = new AxisRecord();
                axisRecords[i] = axisRecord;
                axisRecord.Tag = reader.ReadString(4); //4
                axisRecord.NameId = reader.ReadUInt16(); //2
                axisRecord.Ordering = reader.ReadUInt16(); //2


                //***
                if (designAxisSize > 8)
                {
                    //**Implementations must use the designAxisSize designAxisSize field to determine the start of each record.**
                    //Future minor-version updates of the STAT table may define compatible extensions 
                    //to the axis record format with additional fields.


                    // so skip more ...
                    //
                    //at least there are 8 bytes 
                    reader.BaseStream.Position += (designAxisSize - 8);
                }
            }


            long axisValueOffsets_beginPos = reader.BaseStream.Position = beginPos + offsetToAxisValueOffsets;
            ushort[] axisValueOffsets = reader.ReadArray(axisValueCount, reader.ReadUInt16); // Array of offsets to axis value tables,in bytes from the start of the axis value offsets array.


            //move to axis value record

            AxisValueTableBase[] axisValueTables = new AxisValueTableBase[axisValueCount];
            for (int i = 0; i < axisValueCount; ++i)
            {

                //Axis Value Tables
                //Axis value tables provide details regarding a specific style - attribute value on some specific axis of design variation, 
                //or a combination of design-variation axis values, and the relationship of those values to name elements. 
                //This information can be useful for presenting fonts in application user interfaces.

                //           
                //read each axis table
                ushort offset = axisValueOffsets[i];
                reader.BaseStream.Position = axisValueOffsets_beginPos + offset;

                ushort format = reader.ReadUInt16();//common field of all axis value table
                AxisValueTableBase axisValueTbl = format switch
                {
                    1 => ReadAxisValueFmt1(reader),
                    2 => ReadAxisValueFmt2(reader),
                    3 => ReadAxisValueFmt3(reader),
                    4 => ReadAxisValueFmt4(reader),
                    _ => throw new NotSupportedException(),
                };
                axisValueTables[i] = axisValueTbl;
            }

            return res;
        }

        private AxisValueTableBase ReadAxisValueFmt4(EndianReader reader)
        {
            var res = new AxisValueTableFmt4();
            ushort axisCount = reader.ReadUInt16();
            res.Flags = reader.ReadUInt16();
            res.ValueNameId = reader.ReadUInt16();
            res.ValueRecords = new AxisValueRecord[axisCount];
            for (int i = 0; i < axisCount; ++i)
            {
                res.ValueRecords[i] = new AxisValueRecord(
                    reader.ReadUInt16(),
                    reader.ReadFixed());
            }
            return res;
        }

        private AxisValueTableBase ReadAxisValueFmt3(EndianReader reader)
        {
            var res = new AxisValueTableFmt3();
            res.Index = reader.ReadUInt16();
            res.Flags = reader.ReadUInt16();
            res.ValueNameId = reader.ReadUInt16();
            res.Value = reader.ReadFixed();
            res.LinkedValue = reader.ReadFixed();
            return res;
        }

        private AxisValueTableBase ReadAxisValueFmt2(EndianReader reader)
        {
            var res = new AxisValueTableFmt2();
            res.Index = reader.ReadUInt16();
            res.Flags = reader.ReadUInt16();
            res.ValueNameId = reader.ReadUInt16();
            res.NominalValue = reader.ReadFixed();
            res.RangeMinValue = reader.ReadFixed();
            res.RangeMaxValue = reader.ReadFixed();
            return res;
        }

        private AxisValueTableBase ReadAxisValueFmt1(EndianReader reader)
        {
            var res = new AxisValueTableFmt1();
            res.Index = reader.ReadUInt16();
            res.Flags = reader.ReadUInt16();
            res.ValueNameId = reader.ReadUInt16();
            res.Value = reader.ReadFixed();
            return res;
        }

        public override void Write(EndianWriter writer, StyleAttributesTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
