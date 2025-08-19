using System;
using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GlyphPositioningConverter : GlyphShapingConverter<GlyphPositioningTable>
    {
        protected override void ReadFeatureVariations(EndianReader reader, GlyphPositioningTable instance, long featureVariationsBeginAt)
        {
        }

        protected override void ReadLookup(EndianReader reader, GlyphPositioningTable instance, long lookupTablePos, ushort lookupType, ushort lookupFlags, ushort[] subTableOffsets, ushort markFilteringSet)
        {
            var lookupTable = new LookupTable(lookupFlags, markFilteringSet);
            var subTables = new LookupSubTable[subTableOffsets.Length];
            lookupTable.SubTables = subTables;

            for (int i = 0; i < subTableOffsets.Length; ++i)
            {
                var subTable = ReadSubTable(reader, lookupType, lookupTablePos + subTableOffsets[i]);
                subTable.OwnerGPos = instance;
                subTables[i] = subTable;


                if (lookupType == 9)
                {
                    //temp fix 
                    // (eg. Emoji) => enable long look back
                    instance.EnableLongLookBack = true;
                }
            }


            instance.LookupList.Add(lookupTable);
        }

        private LookupSubTable ReadSubTable(EndianReader reader, ushort lookupType, long subTableStartAt)
        {
            return lookupType switch
            {
                1 => ReadLookupType1(reader, subTableStartAt),
                2 => ReadLookupType2(reader, subTableStartAt),
                3 => ReadLookupType3(reader, subTableStartAt),
                4 => ReadLookupType4(reader, subTableStartAt),
                5 => ReadLookupType5(reader, subTableStartAt),
                6 => ReadLookupType6(reader, subTableStartAt),
                7 => ReadLookupType7(reader, subTableStartAt),
                8 => ReadLookupType8(reader, subTableStartAt),
                9 => ReadLookupType9(reader, subTableStartAt),
                _ => throw new NotSupportedException(),
            };
        }

        private LookupSubTable ReadLookupType9(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            ushort extensionLookupType = reader.ReadUInt16();
            uint extensionOffset = reader.ReadUInt32();
            if (extensionLookupType == 9)
            {
                throw new NotSupportedException();
            }
            // Simply read the lookup table again with updated offsets

            return ReadSubTable(reader, extensionLookupType, subTableStartAt + extensionOffset);
        }

        private LookupSubTable ReadLookupType8(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    return new UnImplementedLookupSubTable(string.Format("GPOS Lookup Table Type 8 Format {0}", format));
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort chainPosRuleSetCount = reader.ReadUInt16();
                        ushort[] chainPosRuleSetOffsetList = reader.ReadUInt16Array(chainPosRuleSetCount);

                        LkSubTableType8Fmt1 subTable = new LkSubTableType8Fmt1();
                        subTable.PosRuleSetTables = CreateMultiplePosRuleSetTables(reader, subTableStartAt, chainPosRuleSetOffsetList);
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);
                        return subTable;
                    }
                case 2:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort backTrackClassDefOffset = reader.ReadUInt16();
                        ushort inputClassDefOffset = reader.ReadUInt16();
                        ushort lookadheadClassDefOffset = reader.ReadUInt16();
                        ushort chainPosClassSetCnt = reader.ReadUInt16();
                        ushort[] chainPosClassSetOffsetArray = reader.ReadUInt16Array(chainPosClassSetCnt);

                        LkSubTableType8Fmt2 subTable = new LkSubTableType8Fmt2();
                        subTable.BackTrackClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + backTrackClassDefOffset);
                        subTable.InputClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + inputClassDefOffset);
                        subTable.LookaheadClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + lookadheadClassDefOffset);

                        //----------
                        PosClassSetTable[] posClassSetTables = new PosClassSetTable[chainPosClassSetCnt];
                        for (int n = 0; n < chainPosClassSetCnt; ++n)
                        {
                            ushort offset = chainPosClassSetOffsetArray[n];
                            if (offset > 0)
                            {
                                posClassSetTables[n] = ReadPosClassSetTable(reader, subTableStartAt + offset);
                            }

                        }
                        subTable.PosClassSetTables = posClassSetTables;
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);

                        return subTable;
                    }
                case 3:
                    {
                        //Chaining Context Positioning Format 3: Coverage-based Chaining Context Glyph Positioning
                        //uint16 	PosFormat 	                    Format identifier-format = 3
                        //uint16 	BacktrackGlyphCount 	        Number of glyphs in the backtracking sequence
                        //Offset16 	Coverage[BacktrackGlyphCount] 	Array of offsets to coverage tables in backtracking sequence, in glyph sequence order
                        //uint16 	InputGlyphCount 	            Number of glyphs in input sequence
                        //Offset16 	Coverage[InputGlyphCount] 	    Array of offsets to coverage tables in input sequence, in glyph sequence order
                        //uint16 	LookaheadGlyphCount 	        Number of glyphs in lookahead sequence
                        //Offset16 	Coverage[LookaheadGlyphCount] 	Array of offsets to coverage tables in lookahead sequence, in glyph sequence order
                        //uint16 	PosCount 	                    Number of PosLookupRecords
                        //struct 	PosLookupRecord[PosCount] 	    Array of PosLookupRecords,in design order

                        var subTable = new LkSubTableType8Fmt3();

                        ushort backtrackGlyphCount = reader.ReadUInt16();
                        ushort[] backtrackCoverageOffsets = reader.ReadUInt16Array(backtrackGlyphCount);
                        ushort inputGlyphCount = reader.ReadUInt16();
                        ushort[] inputGlyphCoverageOffsets = reader.ReadUInt16Array(inputGlyphCount);
                        ushort lookaheadGlyphCount = reader.ReadUInt16();
                        ushort[] lookaheadCoverageOffsets = reader.ReadUInt16Array(lookaheadGlyphCount);

                        ushort posCount = reader.ReadUInt16();
                        subTable.PosLookupRecords = ReadMultiplePosLookupRecords(reader, posCount);

                        subTable.BacktrackCoverages = CoverageConverter.ReadMultiple(reader, subTableStartAt, backtrackCoverageOffsets);
                        subTable.InputGlyphCoverages = CoverageConverter.ReadMultiple(reader, subTableStartAt, inputGlyphCoverageOffsets);
                        subTable.LookaheadCoverages = CoverageConverter.ReadMultiple(reader, subTableStartAt, lookaheadCoverageOffsets);

                        return subTable;
                    }
            }
        }

        private LookupSubTable ReadLookupType7(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    return new UnImplementedLookupSubTable(string.Format("GPOS Lookup Sub Table Type 7 Format {0}", format));
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort posRuleSetCount = reader.ReadUInt16();
                        ushort[] posRuleSetOffsets = reader.ReadUInt16Array(posRuleSetCount);

                        var subTable = new LkSubTableType7Fmt1();
                        subTable.PosRuleSetTables = CreateMultiplePosRuleSetTables(reader, subTableStartAt, posRuleSetOffsets);
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);
                        return subTable;
                    }
                case 2:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort classDefOffset = reader.ReadUInt16();
                        ushort posClassSetCount = reader.ReadUInt16();
                        ushort[] posClassSetOffsets = reader.ReadUInt16Array(posClassSetCount);

                        var subTable = new LkSubTableType7Fmt2();
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);
                        subTable.ClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + classDefOffset);

                        PosClassSetTable[] posClassSetTables = new PosClassSetTable[posClassSetCount];
                        subTable.PosClassSetTables = posClassSetTables;
                        for (int n = 0; n < posClassSetCount; ++n)
                        {
                            ushort offset = posClassSetOffsets[n];
                            if (offset > 0)
                            {
                                posClassSetTables[n] = ReadPosClassSetTable(reader, subTableStartAt + offset);
                            }
                        }
                        return subTable;
                    }
                case 3:
                    {
                        var subTable = new LkSubTableType7Fmt3();
                        ushort glyphCount = reader.ReadUInt16();
                        ushort posCount = reader.ReadUInt16();
                        //read each lookahead record
                        ushort[] coverageOffsets = reader.ReadUInt16Array(glyphCount);
                        subTable.PosLookupRecords = ReadMultiplePosLookupRecords(reader, posCount);
                        subTable.CoverageTables = CoverageConverter.ReadMultiple(reader, subTableStartAt, coverageOffsets);

                        return subTable;
                    }
            }
        }

        private PosClassSetTable ReadPosClassSetTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //--------
            var res = new PosClassSetTable();
            long tableStartAt = reader.BaseStream.Position;
            //
            ushort posClassRuleCnt = reader.ReadUInt16();
            ushort[] posClassRuleOffsets = reader.ReadUInt16Array(posClassRuleCnt);
            res.PosClassRules = new PosClassRule[posClassRuleCnt];
            for (int i = 0; i < posClassRuleOffsets.Length; ++i)
            {
                //move to and read                     
                res.PosClassRules[i] = ReadPosClassRule(reader, tableStartAt + posClassRuleOffsets[i]);
            }
            return res;
        }

        private PosClassRule ReadPosClassRule(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //--------
            PosClassRule posClassRule = new PosClassRule();
            ushort glyphCount = reader.ReadUInt16();
            ushort posCount = reader.ReadUInt16();
            if (glyphCount > 1)
            {
                posClassRule.InputGlyphIds = reader.ReadUInt16Array(glyphCount - 1);
            }

            posClassRule.PosLookupRecords = ReadMultiplePosLookupRecords(reader, posCount);
            return posClassRule;
        }

        private static PosLookupRecord[] ReadMultiplePosLookupRecords(EndianReader reader, ushort count)
        {
            var results = new PosLookupRecord[count];
            for (int n = 0; n < count; ++n)
            {
                results[n] = new PosLookupRecord(reader.ReadUInt16(), reader.ReadUInt16());
            }
            return results;
        }


        private PosRuleSetTable[] CreateMultiplePosRuleSetTables(EndianReader reader, long initPos, ushort[] offsets)
        {
            int j = offsets.Length;
            PosRuleSetTable[] results = new PosRuleSetTable[j];
            for (int i = 0; i < j; ++i)
            {
                results[i] = ReadPosRuleSetTable(reader, initPos + offsets[i]);
            }
            return results;
        }

        private PosRuleSetTable ReadPosRuleSetTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //------------
            var res = new PosRuleSetTable();
            long tableStartAt = reader.BaseStream.Position;
            ushort posRuleCount = reader.ReadUInt16();
            ushort[] posRuleTableOffsets = reader.ReadUInt16Array(posRuleCount);
            int j = posRuleTableOffsets.Length;
            res._posRuleTables = new PosRuleTable[posRuleCount];
            for (int i = 0; i < j; ++i)
            {
                //move to and read
                reader.BaseStream.Seek(tableStartAt + posRuleTableOffsets[i], SeekOrigin.Begin);
                res._posRuleTables[i] = ReadPosRuleTable(reader);

            }
            return res;
        }

        private PosRuleTable ReadPosRuleTable(EndianReader reader)
        {
            var res = new PosRuleTable();
            ushort glyphCount = reader.ReadUInt16();
            ushort posCount = reader.ReadUInt16();
            res._inputGlyphIds = reader.ReadUInt16Array(glyphCount - 1);
            res._posLookupRecords = ReadMultiplePosLookupRecords(reader, posCount);
            return res;
        }

        private LookupSubTable ReadLookupType6(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            if (format != 1)
            {
                return new UnImplementedLookupSubTable(string.Format("GPOS Lookup Sub Table Type 6 Format {0}", format));
            }
            ushort mark1CoverageOffset = reader.ReadUInt16();
            ushort mark2CoverageOffset = reader.ReadUInt16();
            ushort classCount = reader.ReadUInt16();
            ushort mark1ArrayOffset = reader.ReadUInt16();
            ushort mark2ArrayOffset = reader.ReadUInt16();
            //
            var subTable = new LkSubTableType6();
            subTable.MarkCoverage1 = CoverageConverter.Read(reader, subTableStartAt + mark1CoverageOffset);
            subTable.MarkCoverage2 = CoverageConverter.Read(reader, subTableStartAt + mark2CoverageOffset);
            subTable.Mark1ArrayTable = ReadMarkArrayTable(reader, subTableStartAt + mark1ArrayOffset);
            subTable.Mark2ArrayTable = ReadMark2ArrayTable(reader, subTableStartAt + mark2ArrayOffset, classCount);

            return subTable;
        }

        private Mark2ArrayTable ReadMark2ArrayTable(EndianReader reader, long beginAt, ushort classCount)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---
            ushort mark2Count = reader.ReadUInt16();
            ushort[] offsets = reader.ReadUInt16Array(mark2Count * classCount);
            //read mark2 anchors
            AnchorPoint[] anchors = new AnchorPoint[mark2Count * classCount];
            for (int i = 0; i < mark2Count * classCount; ++i)
            {
                anchors[i] = ReadAnchorPoint(reader, beginAt + offsets[i]);
            }
            return new Mark2ArrayTable(classCount, anchors);
        }

        private LookupSubTable ReadLookupType5(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            if (format != 1)
            {
                return new UnImplementedLookupSubTable(string.Format("GPOS Lookup Sub Table Type 5 Format {0}", format));
            }
            ushort markCoverageOffset = reader.ReadUInt16(); //from beginning of MarkLigPos subtable
            ushort ligatureCoverageOffset = reader.ReadUInt16();
            ushort classCount = reader.ReadUInt16();
            ushort markArrayOffset = reader.ReadUInt16();
            ushort ligatureArrayOffset = reader.ReadUInt16();
            //-----------------------
            var subTable = new LkSubTableType5();
            subTable.MarkCoverage = CoverageConverter.Read(reader, subTableStartAt + markCoverageOffset);
            subTable.LigatureCoverage = CoverageConverter.Read(reader, subTableStartAt + ligatureCoverageOffset);
            subTable.MarkArrayTable = ReadMarkArrayTable(reader, subTableStartAt + markArrayOffset);

            reader.BaseStream.Seek(subTableStartAt + ligatureArrayOffset, SeekOrigin.Begin);
            subTable.LigatureArrayTable = ReadLigatureArrayTable(reader, classCount);

            return subTable;
        }

        private LigatureArrayTable ReadLigatureArrayTable(EndianReader reader, ushort classCount)
        {
            var res = new LigatureArrayTable();
            long startPos = reader.BaseStream.Position;
            ushort ligatureCount = reader.ReadUInt16();
            ushort[] offsets = reader.ReadUInt16Array(ligatureCount);

            res._ligatures = new LigatureAttachTable[ligatureCount];

            for (int i = 0; i < ligatureCount; ++i)
            {
                //each ligature table
                reader.BaseStream.Seek(startPos + offsets[i], SeekOrigin.Begin);
                res._ligatures[i] = ReadLigatureAttachTable(reader, classCount);
            }
            return res;
        }

        private LigatureAttachTable ReadLigatureAttachTable(EndianReader reader, ushort classCount)
        {
            var table = new LigatureAttachTable();
            ushort componentCount = reader.ReadUInt16();
            var componentRecs = new ComponentRecord[componentCount];
            table._records = componentRecs;
            for (int i = 0; i < componentCount; ++i)
            {
                componentRecs[i] = new ComponentRecord(
                    reader.ReadUInt16Array(classCount));
            }
            return table;
        }

        private LookupSubTable ReadLookupType4(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            if (format != 1)
            {
                return new UnImplementedLookupSubTable(string.Format("GPOS Lookup Sub Table Type 4 Format {0}", format));
            }
            ushort markCoverageOffset = reader.ReadUInt16(); //offset from
            ushort baseCoverageOffset = reader.ReadUInt16();
            ushort markClassCount = reader.ReadUInt16();
            ushort markArrayOffset = reader.ReadUInt16();
            ushort baseArrayOffset = reader.ReadUInt16();

            //read mark array table
            var lookupType4 = new LkSubTableType4();
            lookupType4.MarkCoverageTable = CoverageConverter.Read(reader, subTableStartAt + markCoverageOffset);
            lookupType4.BaseCoverageTable = CoverageConverter.Read(reader, subTableStartAt + baseCoverageOffset);
            lookupType4.MarkArrayTable = ReadMarkArrayTable(reader, subTableStartAt + markArrayOffset);
            lookupType4.BaseArrayTable = ReadBaseArrayTable(reader, subTableStartAt + baseArrayOffset, markClassCount);
            return lookupType4;
        }

        private BaseArrayTable ReadBaseArrayTable(EndianReader reader, long beginAt, ushort classCount)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---
            var baseArrTable = new BaseArrayTable();
            ushort baseCount = reader.ReadUInt16();
            baseArrTable._records = new BaseRecord[baseCount];
            // Read all baseAnchorOffsets in one go
            ushort[] baseAnchorOffsets = reader.ReadUInt16Array(classCount * baseCount);
            for (int i = 0; i < baseCount; ++i)
            {
                var anchors = new AnchorPoint[classCount];
                var baseRec = new BaseRecord(anchors);

                //each base has anchor point for mark glyph'class
                for (int n = 0; n < classCount; ++n)
                {
                    ushort offset = baseAnchorOffsets[i * classCount + n];
                    if (offset <= 0)
                    {
                        //TODO: review here 
                        //bug?
                        continue;
                    }
                    anchors[n] = ReadAnchorPoint(reader, beginAt + offset);
                }

                baseArrTable._records[i] = baseRec;
            }
            return baseArrTable;
        }

        private MarkArrayTable ReadMarkArrayTable(EndianReader reader, long v)
        {
            var res = new MarkArrayTable();
            long markTableBeginAt = reader.BaseStream.Position;
            ushort markCount = reader.ReadUInt16();
            res._records = new MarkRecord[markCount];
            for (int i = 0; i < markCount; ++i)
            {
                //1 mark : 1 anchor
                res._records[i] = new MarkRecord(
                    reader.ReadUInt16(),//mark class
                    reader.ReadUInt16()); //offset to anchor table
            }
            //---------------------------
            //read anchor
            res._anchorPoints = new AnchorPoint[markCount];
            for (int i = 0; i < markCount; ++i)
            {
                var markRec = res._records[i];
                //bug?
                if (markRec.offset < 0)
                {
                    //TODO: review here
                    //found err on Tahoma
                    continue;
                }
                //read table detail
                res._anchorPoints[i] = ReadAnchorPoint(reader, markTableBeginAt + markRec.offset);
            }
            return res;
        }

        private static AnchorPoint ReadAnchorPoint(EndianReader reader, long beginAt)
        {
            var anchorPoint = new AnchorPoint();
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            switch (anchorPoint.format = reader.ReadUInt16())
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        anchorPoint.xcoord = reader.ReadInt16();
                        anchorPoint.ycoord = reader.ReadInt16();
                    }
                    break;
                case 2:
                    {
                        anchorPoint.xcoord = reader.ReadInt16();
                        anchorPoint.ycoord = reader.ReadInt16();
                        anchorPoint.refGlyphContourPoint = reader.ReadUInt16();

                    }
                    break;
                case 3:
                    {
                        anchorPoint.xcoord = reader.ReadInt16();
                        anchorPoint.ycoord = reader.ReadInt16();
                        anchorPoint.xdeviceTableOffset = reader.ReadUInt16();
                        anchorPoint.ydeviceTableOffset = reader.ReadUInt16();
                    }
                    break;
            }
            return anchorPoint;
        }

        private LookupSubTable ReadLookupType3(EndianReader reader, long subTableStartAt)
        {
            return new UnImplementedLookupSubTable("GPOS Lookup Table Type 3");
        }

        private LookupSubTable ReadLookupType2(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    return new UnImplementedLookupSubTable(string.Format("GPOS Lookup Table Type 2 Format {0}", format));
                case 1:
                    {
                        ushort coverage = reader.ReadUInt16();
                        ushort value1Format = reader.ReadUInt16();
                        ushort value2Format = reader.ReadUInt16();
                        ushort pairSetCount = reader.ReadUInt16();
                        ushort[] pairSetOffsetArray = reader.ReadUInt16Array(pairSetCount);
                        PairSetTable[] pairSetTables = new PairSetTable[pairSetCount];
                        for (int n = 0; n < pairSetCount; ++n)
                        {
                            reader.BaseStream.Seek(subTableStartAt + pairSetOffsetArray[n], SeekOrigin.Begin);
                            var pairSetTable = ReadPairSet(reader, value1Format, value2Format);
                            pairSetTables[n] = pairSetTable;
                        }
                        var subTable = new LkSubTableType2Fmt1(pairSetTables);
                        //coverage
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return subTable;
                    }
                case 2:
                    {
                        ushort coverage = reader.ReadUInt16();
                        ushort value1Format = reader.ReadUInt16();
                        ushort value2Format = reader.ReadUInt16();
                        ushort classDef1_offset = reader.ReadUInt16();
                        ushort classDef2_offset = reader.ReadUInt16();
                        ushort class1Count = reader.ReadUInt16();
                        ushort class2Count = reader.ReadUInt16();

                        Lk2Class1Record[] class1Records = new Lk2Class1Record[class1Count];
                        for (int c1 = 0; c1 < class1Count; ++c1)
                        {
                            //for each c1 record

                            Lk2Class2Record[] class2Records = new Lk2Class2Record[class2Count];
                            for (int c2 = 0; c2 < class2Count; ++c2)
                            {
                                class2Records[c2] = new Lk2Class2Record(
                                      ReadValueRecord(reader, value1Format),
                                      ReadValueRecord(reader, value2Format));
                            }
                            class1Records[c1] = new Lk2Class1Record(class2Records);
                        }

                        var subTable = new LkSubTableType2Fmt2(class1Records,
                                            GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + classDef1_offset),
                                            GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + classDef2_offset));


                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return subTable;
                    }
            }
        }


        private PairSetTable ReadPairSet(EndianReader reader, ushort v1Format, ushort v2Format)
        {
            var res = new PairSetTable();

            ushort rowCount = reader.ReadUInt16();
            res._pairSets = new PairSet[rowCount];
            for (int i = 0; i < rowCount; ++i)
            {
                //GlyphID 	    SecondGlyph 	GlyphID of second glyph in the pair-first glyph is listed in the Coverage table
                //ValueRecord 	Value1 	        Positioning data for the first glyph in the pair
                //ValueRecord 	Value2 	        Positioning data for the second glyph in the pair
                ushort secondGlyph = reader.ReadUInt16();
                var v1 = ReadValueRecord(reader, v1Format);
                var v2 = ReadValueRecord(reader, v2Format);
                res._pairSets[i] = new PairSet(secondGlyph, v1, v2);
            }
            return res;
        }

        private LookupSubTable ReadLookupType1(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            ushort coverage = reader.ReadUInt16();
            ushort valueFormat = reader.ReadUInt16();
            switch (format)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        var valueRecord = ReadValueRecord(reader, valueFormat);
                        var coverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return new LkSubTableType1(coverageTable, valueRecord);
                    }
                case 2:
                    {
                        ushort valueCount = reader.ReadUInt16();
                        var valueRecords = new PairValueRecord[valueCount];
                        for (int n = 0; n < valueCount; ++n)
                        {
                            valueRecords[n] = ReadValueRecord(reader, valueFormat);
                        }
                        var coverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return new LkSubTableType1(coverageTable, valueRecords);
                    }
            }
        }

        static bool HasFormat(ushort value, int flags)
        {
            return (value & flags) == flags;
        }

        private static PairValueRecord ReadValueRecord(EndianReader reader, ushort valueFormat)
        {
            var res = new PairValueRecord();
            res.valueFormat = valueFormat;
            if (HasFormat(valueFormat, PairValueRecord.FMT_XPlacement))
            {
                res.XPlacement = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_YPlacement))
            {
                res.YPlacement = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_XAdvance))
            {
                res.XAdvance = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_YAdvance))
            {
                res.YAdvance = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_XPlaDevice))
            {
                res.XPlaDevice = reader.ReadUInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_YPlaDevice))
            {
                res.YPlaDevice = reader.ReadUInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_XAdvDevice))
            {
                res.XAdvDevice = reader.ReadUInt16();
            }
            if (HasFormat(valueFormat, PairValueRecord.FMT_YAdvDevice))
            {
                res.YAdvDevice = reader.ReadUInt16();
            }
            return res;
        }
    }
}
