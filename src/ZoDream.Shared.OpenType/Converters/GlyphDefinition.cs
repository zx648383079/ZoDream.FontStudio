using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GlyphDefinitionConverter : TypefaceConverter<GlyphDefinitionTable>
    {
        public override GlyphDefinitionTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new GlyphDefinitionTable();
            var startAt = reader.BaseStream.Position;
            res.MajorVersion = reader.ReadUInt16();
            res.MinorVersion = reader.ReadUInt16();
            //
            ushort glyphClassDefOffset = reader.ReadUInt16();
            ushort attachListOffset = reader.ReadUInt16();
            ushort ligCaretListOffset = reader.ReadUInt16();
            ushort markAttachClassDefOffset = reader.ReadUInt16();
            ushort markGlyphSetsDefOffset = 0;
            uint itemVarStoreOffset = 0;
            //
            switch (res.MinorVersion)
            {
                default:
                    break;
                case 0:
                    break;
                case 2:
                    markGlyphSetsDefOffset = reader.ReadUInt16();
                    break;
                case 3:
                    markGlyphSetsDefOffset = reader.ReadUInt16();
                    itemVarStoreOffset = reader.ReadUInt32();
                    break;
            }
            //---------------


            res.GlyphClassDef = (glyphClassDefOffset == 0) ? null : ReadClassDefTable(reader, startAt + glyphClassDefOffset);
            res.AttachmentListTable = (attachListOffset == 0) ? null : ReadAttachmentListTable(reader, startAt + attachListOffset);
            res.LigCaretList = (ligCaretListOffset == 0) ? null : ReadLigCaretList(reader, startAt + ligCaretListOffset);

            //A Mark Attachment Class Definition Table defines the class to which a mark glyph may belong.
            //This table uses the same format as the Class Definition table (for details, see the chapter, Common Table Formats ).


#if DEBUG
            if (markAttachClassDefOffset == 2)
            {
                //temp debug invalid font                
                res.MarkAttachmentClassDef = (markAttachClassDefOffset == 0) ? null : ReadClassDefTable(reader, reader.BaseStream.Position);
            }
            else
            {
                res.MarkAttachmentClassDef = (markAttachClassDefOffset == 0) ? null : ReadClassDefTable(reader, startAt + markAttachClassDefOffset);
            }
#else
            res.MarkAttachmentClassDef = (markAttachClassDefOffset == 0) ? null : ReadClassDefTable(reader, startAt + markAttachClassDefOffset);
#endif

            res.MarkGlyphSetsTable = (markGlyphSetsDefOffset == 0) ? null : ReadMarkGlyphSetsTable(reader, startAt + markGlyphSetsDefOffset);

            if (itemVarStoreOffset != 0)
            {
                //not supported
                reader.BaseStream.Seek(startAt + itemVarStoreOffset, SeekOrigin.Begin);
            }

            return res;
        }

        private MarkGlyphSetsTable ReadMarkGlyphSetsTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            var markGlyphSetsTable = new MarkGlyphSetsTable();
            markGlyphSetsTable.Format = reader.ReadUInt16();
            ushort markSetCount = reader.ReadUInt16();
            uint[] coverageOffset = markGlyphSetsTable.CoverageOffset = new uint[markSetCount];
            for (int i = 0; i < markSetCount; ++i)
            {
                //Note that the array of offsets for the Coverage tables uses ULONG 
                coverageOffset[i] = reader.ReadUInt32();//
            }

            return markGlyphSetsTable;
        }

        private LigCaretList ReadLigCaretList(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            var ligcaretList = new LigCaretList();
            ushort coverageOffset = reader.ReadUInt16();
            ushort ligGlyphCount = reader.ReadUInt16();
            ushort[] ligGlyphOffsets = reader.ReadArray(ligGlyphCount, reader.ReadUInt16);
            LigGlyph[] ligGlyphs = new LigGlyph[ligGlyphCount];
            for (int i = 0; i < ligGlyphCount; ++i)
            {
                ligGlyphs[i] = ReadLigGlyph(reader, beginAt + ligGlyphOffsets[i]);
            }
            ligcaretList.LigGlyphs = ligGlyphs;
            ligcaretList.CoverageTable = CoverageConverter.Read(reader, beginAt + coverageOffset);
            return ligcaretList;
        }

        private LigGlyph ReadLigGlyph(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            var ligGlyph = new LigGlyph();
            ushort caretCount = reader.ReadUInt16();
            ligGlyph.CaretValueOffsets = reader.ReadArray(caretCount, reader.ReadUInt16);
            return ligGlyph;
        }

        private AttachmentListTable ReadAttachmentListTable(EndianReader reader, long beginAt)
        {
            var attachmentListTable = new AttachmentListTable();
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            ushort coverageOffset = reader.ReadUInt16();
            ushort glyphCount = reader.ReadUInt16();
            ushort[] attachPointOffsets = reader.ReadArray(glyphCount, reader.ReadUInt16);
            //-----------------------
            attachmentListTable.CoverageTable = CoverageConverter.Read(reader, beginAt + coverageOffset);
            attachmentListTable.AttachPoints = new ushort[glyphCount][];
            for (int i = 0; i < glyphCount; ++i)
            {
                reader.BaseStream.Seek(beginAt + attachPointOffsets[i], SeekOrigin.Begin);
                ushort pointCount = reader.ReadUInt16();
                attachmentListTable.AttachPoints[i] = reader.ReadArray(pointCount, reader.ReadUInt16);
            }

            return attachmentListTable;
        }

        public static ClassDefTable ReadClassDefTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            var classDefTable = new ClassDefTable();
            switch (classDefTable.Format = reader.ReadUInt16())
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        classDefTable.startGlyph = reader.ReadUInt16();
                        ushort glyphCount = reader.ReadUInt16();
                        classDefTable.classValueArray = reader.ReadArray(glyphCount, reader.ReadUInt16);
                    }
                    break;
                case 2:
                    {
                        ushort classRangeCount = reader.ReadUInt16();
                        ClassRangeRecord[] records = new ClassRangeRecord[classRangeCount];
                        for (int i = 0; i < classRangeCount; ++i)
                        {
                            records[i] = new ClassRangeRecord(
                                reader.ReadUInt16(), //start glyph id
                                reader.ReadUInt16(), //end glyph id
                                reader.ReadUInt16()); //classNo
                        }
                        classDefTable.records = records;
                    }
                    break;
            }
            return classDefTable;
        }

        public override void Write(EndianWriter writer, GlyphDefinitionTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
