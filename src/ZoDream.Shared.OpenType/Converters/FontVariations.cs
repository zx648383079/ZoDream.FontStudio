using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class FontVariationsConverter : TypefaceConverter<FontVariationsTable>
    {
        public override FontVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new FontVariationsTable();
            var majorVersion = reader.ReadUInt16();
            var minorVersion = reader.ReadUInt16();
            var axesArrayOffset = reader.ReadUInt16();
            var reserved = reader.ReadUInt16();//set to 2 
            var axisCount = reader.ReadUInt16();
            var axisSize = reader.ReadUInt16();
            var instanceCount = reader.ReadUInt16();
            var instanceSize = reader.ReadUInt16();
            res.VariableAxisRecords = reader.ReadArray(axisCount, () => {
                var end = reader.Position + axisSize;
                var varAxisRecord = new VariableAxisRecord()
                {
                    AxisTag = reader.ReadString(4),//4
                    MinValue = reader.ReadFixed(),//4
                    DefaultValue = reader.ReadFixed(),//4
                    MaxValue = reader.ReadFixed(),//4
                    Flags = reader.ReadUInt16(),//2
                    AxisNameID = reader.ReadUInt16(),//2
                };
                reader.Position = end;
                return varAxisRecord;
            });
            res.InstanceRecords = reader.ReadArray(instanceCount, () => {
                var end = reader.Position + instanceSize;
                var instanceRecord = new InstanceRecord()
                {
                    SubfamilyNameID = reader.ReadUInt16(),
                    Flags = reader.ReadUInt16(),
                    Coordinates = reader.ReadArray(axisCount, reader.ReadFixed),
                };
                if (reader.Position < end)
                {
                    instanceRecord.PostScriptNameID = reader.ReadUInt16();
                }
                reader.Position = end;
                return instanceRecord;
            });
            return res;
        }

        public override void Write(EndianWriter writer, FontVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
