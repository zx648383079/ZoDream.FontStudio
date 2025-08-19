using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class JustificationConverter : TypefaceConverter<JustificationTable>
    {
        public override JustificationTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new JustificationTable();
            long tableStartAt = reader.BaseStream.Position;
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            ushort jstfScriptCount = reader.ReadUInt16();

            var recs = reader.ReadRecord(jstfScriptCount);

            res.ScriptTables = new JstfScriptTable[recs.Length];
            for (int i = 0; i < recs.Length; ++i)
            {
                var rec = recs[i];
                reader.BaseStream.Position = tableStartAt + rec.Offset;

                JstfScriptTable jstfScriptTable = ReadJstfScriptTable(reader);
                jstfScriptTable.ScriptTag = rec.Tag;
                res.ScriptTables[i] = jstfScriptTable;
            }
            return res;
        }


        private JstfScriptTable ReadJstfScriptTable(EndianReader reader)
        {
            var jstfScriptTable = new JstfScriptTable();

            long tableStartAt = reader.BaseStream.Position;

            ushort extenderGlyphOffset = reader.ReadUInt16();
            ushort defJstfLangSysOffset = reader.ReadUInt16();
            ushort jstfLangSysCount = reader.ReadUInt16();

            if (jstfLangSysCount > 0)
            {
                var recs = new JstfLangSysRecord[jstfLangSysCount];
                for (int i = 0; i < jstfLangSysCount; ++i)
                {
                    recs[i] = ReadJstfLangSysRecord(reader);
                }
                jstfScriptTable.Other = recs;
            }


            if (extenderGlyphOffset > 0)
            {
                reader.BaseStream.Position = tableStartAt + extenderGlyphOffset;
                jstfScriptTable.ExtenderGlyphs = reader.ReadArray(reader.ReadUInt16(), reader.ReadUInt16);
            }

            if (defJstfLangSysOffset > 0)
            {
                reader.BaseStream.Position = tableStartAt + defJstfLangSysOffset;
                jstfScriptTable.DefaultLangSys = ReadJstfLangSysRecord(reader);
            }
            return jstfScriptTable;
        }

        private JstfLangSysRecord ReadJstfLangSysRecord(EndianReader reader)
        {
            long tableStartAt = reader.BaseStream.Position;
            ushort jstfPriorityCount = reader.ReadUInt16();
            ushort[] jstfPriorityOffsets = reader.ReadArray(jstfPriorityCount, reader.ReadUInt16);

            var jstPriorities = new JstfPriority[jstfPriorityCount];

            for (int i = 0; i < jstfPriorityOffsets.Length; ++i)
            {
                reader.BaseStream.Position = tableStartAt + jstfPriorityOffsets[i];
                jstPriorities[i] = ReadJstfPriority(reader);
            }

            return new JstfLangSysRecord() { JstfPriority = jstPriorities };
        }

        private JstfPriority ReadJstfPriority(EndianReader reader)
        {
            return new JstfPriority()
            {
                ShrinkageEnableGSUB = reader.ReadUInt16(),
                ShrinkageDisableGSUB = reader.ReadUInt16(),

                ShrinkageEnableGPOS = reader.ReadUInt16(),
                ShrinkageDisableGPOS = reader.ReadUInt16(),

                ShrinkageJstfMax = reader.ReadUInt16(),

                ExtensionEnableGSUB = reader.ReadUInt16(),
                ExtensionDisableGSUB = reader.ReadUInt16(),

                ExtensionEnableGPOS = reader.ReadUInt16(),
                ExtensionDisableGPOS = reader.ReadUInt16(),

                ExtensionJstfMax = reader.ReadUInt16(),
            };
        }


        public override void Write(EndianWriter writer, JustificationTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
