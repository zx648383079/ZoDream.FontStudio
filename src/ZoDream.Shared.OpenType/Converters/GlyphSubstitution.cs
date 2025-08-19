using System;
using System.IO;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GlyphSubstitutionConverter : GlyphShapingConverter<GlyphSubstitutionTable>
    {
        protected override void ReadFeatureVariations(EndianReader reader, 
            GlyphSubstitutionTable instance, long featureVariationsBeginAt)
        {
            
        }

        protected override void ReadLookup(EndianReader reader, GlyphSubstitutionTable instance, 
            long lookupTablePos, ushort lookupType, 
            ushort lookupFlags, ushort[] subTableOffsets, ushort markFilteringSet)
        {
            LookupTable lookupTable = new LookupTable(lookupFlags, markFilteringSet);
            LookupSubTable[] subTables = new LookupSubTable[subTableOffsets.Length];
            lookupTable.SubTables = subTables;

            for (int i = 0; i < subTableOffsets.Length; ++i)
            {
                var subTable = ReadSubTable(reader, lookupType, lookupTablePos + subTableOffsets[i]);
                subTable.OwnerGSub = instance;
                subTables[i] = subTable;
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
                _ => new UnImplementedLookupSubTable(string.Format("GSUB Lookup Type {0}", lookupType)),
            };
        }

        private LookupSubTable ReadLookupType8(EndianReader reader, long subTableStartAt)
        {
            throw new NotImplementedException();
        }

        private LookupSubTable ReadLookupType7(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            ushort extensionLookupType = reader.ReadUInt16();
            uint extensionOffset = reader.ReadUInt32();
            if (extensionLookupType == 7)
            {
                throw new NotSupportedException();
            }
            // Simply read the lookup table again with updated offsets 
            return ReadSubTable(reader, extensionLookupType, subTableStartAt + extensionOffset);
        }

        private LookupSubTable ReadLookupType6(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        var subTable = new LkSubTableT6Fmt1();
                        ushort coverage = reader.ReadUInt16();
                        ushort chainSubRulesetCount = reader.ReadUInt16();
                        ushort[] chainSubRulesetOffsets = reader.ReadUInt16Array(chainSubRulesetCount);
                        ChainSubRuleSetTable[] subRuleSets = subTable.SubRuleSets = new ChainSubRuleSetTable[chainSubRulesetCount];
                        for (int n = 0; n < chainSubRulesetCount; ++n)
                        {
                            subRuleSets[n] = ReadChainSubRuleSetTable(reader, subTableStartAt + chainSubRulesetOffsets[n]);
                        }
                        //----------------------------
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return subTable;
                    }
                case 2:
                    {                        
                        var subTable = new LkSubTableT6Fmt2();
                        ushort coverage = reader.ReadUInt16();
                        ushort backtrackClassDefOffset = reader.ReadUInt16();
                        ushort inputClassDefOffset = reader.ReadUInt16();
                        ushort lookaheadClassDefOffset = reader.ReadUInt16();
                        ushort chainSubClassSetCount = reader.ReadUInt16();
                        ushort[] chainSubClassSetOffsets = reader.ReadUInt16Array(chainSubClassSetCount);
                        //
                        subTable.BacktrackClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + backtrackClassDefOffset);
                        subTable.InputClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + inputClassDefOffset);
                        subTable.LookaheadClassDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + lookaheadClassDefOffset);
                        if (chainSubClassSetCount != 0)
                        {
                            ChainSubClassSet[] chainSubClassSets = subTable.ChainSubClassSets = new ChainSubClassSet[chainSubClassSetCount];
                            for (int n = 0; n < chainSubClassSetCount; ++n)
                            {
                                ushort offset = chainSubClassSetOffsets[n];
                                if (offset > 0)
                                {
                                    chainSubClassSets[n] = ReadChainSubClassSet(reader, subTableStartAt + offset);
                                }
                            }
                        }

                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return subTable;
                    }
                case 3:
                    {
                        LkSubTableT6Fmt3 subTable = new LkSubTableT6Fmt3();
                        ushort backtrackingGlyphCount = reader.ReadUInt16();
                        ushort[] backtrackingCoverageOffsets = reader.ReadUInt16Array(backtrackingGlyphCount);
                        ushort inputGlyphCount = reader.ReadUInt16();
                        ushort[] inputGlyphCoverageOffsets = reader.ReadUInt16Array(inputGlyphCount);
                        ushort lookAheadGlyphCount = reader.ReadUInt16();
                        ushort[] lookAheadCoverageOffsets = reader.ReadUInt16Array(lookAheadGlyphCount);
                        ushort substCount = reader.ReadUInt16();
                        subTable.SubstLookupRecords = ReadSubstLookupRecords(reader, substCount);

                        subTable.BacktrackingCoverages = CoverageConverter.ReadMultiple(reader, subTableStartAt, backtrackingCoverageOffsets);
                        subTable.InputCoverages = CoverageConverter.ReadMultiple(reader, subTableStartAt, inputGlyphCoverageOffsets);
                        subTable.LookaheadCoverages = CoverageConverter.ReadMultiple(reader, subTableStartAt, lookAheadCoverageOffsets);

                        return subTable;
                    }
            }
        }

        private ChainSubClassSet ReadChainSubClassSet(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            ChainSubClassSet chainSubClassSet = new ChainSubClassSet();
            ushort count = reader.ReadUInt16();
            ushort[] subClassRuleOffsets = reader.ReadUInt16Array(count);

            ChainSubClassRuleTable[] subClassRuleTables = chainSubClassSet.subClassRuleTables = new ChainSubClassRuleTable[count];
            for (int i = 0; i < count; ++i)
            {
                subClassRuleTables[i] = ReadChainSubClassRuleTable(reader, beginAt + subClassRuleOffsets[i]);
            }
            return chainSubClassSet;
        }

        private ChainSubClassRuleTable ReadChainSubClassRuleTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            ChainSubClassRuleTable subClassRuleTable = new ChainSubClassRuleTable();
            ushort backtrackingCount = reader.ReadUInt16();
            subClassRuleTable.backtrakcingClassDefs = reader.ReadUInt16Array(backtrackingCount);
            ushort inputGlyphCount = reader.ReadUInt16();
            subClassRuleTable.inputClassDefs = reader.ReadUInt16Array(inputGlyphCount - 1);//** -1
            ushort lookaheadGlyphCount = reader.ReadUInt16();
            subClassRuleTable.lookaheadClassDefs = reader.ReadUInt16Array(lookaheadGlyphCount);
            ushort substCount = reader.ReadUInt16();
            subClassRuleTable.subsLookupRecords = ReadSubstLookupRecords(reader, substCount);

            return subClassRuleTable;
        }

        private ChainSubRuleSetTable ReadChainSubRuleSetTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---
            var table = new ChainSubRuleSetTable();
            ushort subRuleCount = reader.ReadUInt16();
            ushort[] subRuleOffsets = reader.ReadUInt16Array(subRuleCount);
            ChainSubRuleSubTable[] chainSubRuleSubTables = table._chainSubRuleSubTables = new ChainSubRuleSubTable[subRuleCount];
            for (int i = 0; i < subRuleCount; ++i)
            {
                chainSubRuleSubTables[i] = ReadChainSubRuleSubTable(reader, beginAt + subRuleOffsets[i]);
            }

            return table;
        }

        private ChainSubRuleSubTable ReadChainSubRuleSubTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            //------------
            ChainSubRuleSubTable subRuleTable = new ChainSubRuleSubTable();
            ushort backtrackGlyphCount = reader.ReadUInt16();
            subRuleTable.backTrackingGlyphs = reader.ReadUInt16Array(backtrackGlyphCount);
            //--------
            ushort inputGlyphCount = reader.ReadUInt16();
            subRuleTable.inputGlyphs = reader.ReadUInt16Array(inputGlyphCount - 1);//*** start with second glyph, so -1
                                                                                          //----------
            ushort lookaheadGlyphCount = reader.ReadUInt16();
            subRuleTable.lookaheadGlyphs = reader.ReadUInt16Array(lookaheadGlyphCount);
            //------------
            ushort substCount = reader.ReadUInt16();
            subRuleTable.substLookupRecords = ReadSubstLookupRecords(reader, substCount);

            return subRuleTable;
        }

        private LookupSubTable ReadLookupType5(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort substFormat = reader.ReadUInt16();
            switch (substFormat)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        //ContextSubstFormat1 Subtable
                        //Table 14
                        //Type  	Name 	            Description
                        //uint16 	substFormat 	    Format identifier: format = 1
                        //Offset16 	coverageOffset 	    Offset to Coverage table, from beginning of substitution subtable
                        //uint16 	subRuleSetCount 	Number of SubRuleSet tables — must equal glyphCount in Coverage table***
                        //Offset16 	subRuleSetOffsets[subRuleSetCount] 	Array of offsets to SubRuleSet tables.
                        //                              Offsets are from beginning of substitution subtable, ordered by Coverage index

                        LkSubTableT5Fmt1 fmt1 = new LkSubTableT5Fmt1();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort subRuleSetCount = reader.ReadUInt16();
                        ushort[] subRuleSetOffsets = reader.ReadUInt16Array(subRuleSetCount);

                        fmt1.coverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);
                        fmt1.subRuleSets = new LkSubT5Fmt1_SubRuleSet[subRuleSetCount];

                        for (int i = 0; i < subRuleSetCount; ++i)
                        {
                            fmt1.subRuleSets[i] = ReadSubRuleSet(reader, subTableStartAt + subRuleSetOffsets[i]);
                        }

                        return fmt1;
                    }
                case 2:
                    {

                        var fmt2 = new LkSubTableT5Fmt2();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort classDefOffset = reader.ReadUInt16();
                        ushort subClassSetCount = reader.ReadUInt16();
                        ushort[] subClassSetOffsets = reader.ReadUInt16Array(subClassSetCount);
                        //
                        fmt2.coverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);
                        fmt2.classDef = GlyphDefinitionConverter.ReadClassDefTable(reader, subTableStartAt + classDefOffset);

                        var subClassSets = new LkSubT5Fmt2_SubClassSet[subClassSetCount];
                        fmt2.subClassSets = subClassSets;
                        for (int i = 0; i < subClassSetCount; ++i)
                        {
                            subClassSets[i] = ReadSubClassSet(reader, subTableStartAt + subClassSetOffsets[i]);
                        }

                        return fmt2;
                    }

                case 3:
                    {
                        return new UnImplementedLookupSubTable("GSUB,Lookup Subtable Type 5,Fmt3");
                    }

            }
        }

        private LkSubT5Fmt2_SubClassSet ReadSubClassSet(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            LkSubT5Fmt2_SubClassSet fmt2 = new LkSubT5Fmt2_SubClassSet();
            ushort subClassRuleCount = reader.ReadUInt16();
            ushort[] subClassRuleOffsets = reader.ReadUInt16Array(subClassRuleCount);
            fmt2.subClassRules = new LkSubT5Fmt2_SubClassRule[subClassRuleCount];
            for (int i = 0; i < subClassRuleCount; ++i)
            {
                fmt2.subClassRules[i] = ReadSubClassRule(reader, beginAt + subClassRuleOffsets[i]);
            }

            return fmt2;
        }

        private LkSubT5Fmt2_SubClassRule ReadSubClassRule(EndianReader reader, long pos)
        {
            var res = new LkSubT5Fmt2_SubClassRule();
            reader.BaseStream.Seek(pos, SeekOrigin.Begin);

            ushort glyphCount = reader.ReadUInt16();
            ushort substitutionCount = reader.ReadUInt16();
            res.inputSequence = reader.ReadUInt16Array(glyphCount - 1);
            res.substRecords = ReadSubstLookupRecords(reader, substitutionCount);
            return res;
        }

        private LkSubT5Fmt1_SubRuleSet ReadSubRuleSet(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            LkSubT5Fmt1_SubRuleSet subRuleSet = new LkSubT5Fmt1_SubRuleSet();

            //SubRuleSet table: All contexts beginning with the same glyph
            //Table 15
            //Type        Name                  Description
            //uint16      subRuleCount          Number of SubRule tables
            //Offset16    subRuleOffsets[subRuleCount]    Array of offsets to SubRule tables. Offsets are from beginning of SubRuleSet table, ordered by preference

            ushort subRuleCount = reader.ReadUInt16();

            ushort[] subRuleOffsets = reader.ReadUInt16Array(subRuleCount);
            var subRules = new LkSubT5Fmt1_SubRule[subRuleCount];
            subRuleSet.subRules = subRules;
            for (int i = 0; i < subRuleCount; ++i)
            {

                subRules[i] = ReadSubRule(reader, beginAt + subRuleOffsets[i]);
            }
            return subRuleSet;
        }

        private LkSubT5Fmt1_SubRule ReadSubRule(EndianReader reader, long pos)
        {
            var res = new LkSubT5Fmt1_SubRule();
            reader.BaseStream.Seek(pos, SeekOrigin.Begin);
            ushort glyphCount = reader.ReadUInt16();
            ushort substitutionCount = reader.ReadUInt16();

            res.inputSequence = reader.ReadUInt16Array(glyphCount - 1);
            res.substRecords = ReadSubstLookupRecords(reader, substitutionCount);
            return res;
        }

        private SubstLookupRecord[] ReadSubstLookupRecords(EndianReader reader, ushort ncount)
        {
            var results = new SubstLookupRecord[ncount];
            for (int i = 0; i < ncount; ++i)
            {
                results[i] = new SubstLookupRecord(reader.ReadUInt16(), reader.ReadUInt16());
            }
            return results;
        }

        private LookupSubTable ReadLookupType4(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort ligSetCount = reader.ReadUInt16();
                        ushort[] ligSetOffsets = reader.ReadUInt16Array(ligSetCount);
                        LkSubTableT4 subTable = new LkSubTableT4();
                        LigatureSetTable[] ligSetTables = subTable.LigatureSetTables = new LigatureSetTable[ligSetCount];
                        for (int n = 0; n < ligSetCount; ++n)
                        {
                            ligSetTables[n] = ReadLigatureSetTable(reader, subTableStartAt + ligSetOffsets[n]);
                        }
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);
                        return subTable;
                    }
            }
        }

        private LigatureSetTable ReadLigatureSetTable(EndianReader reader, long beginAt)
        {
            LigatureSetTable ligSetTable = new LigatureSetTable();
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            ushort ligCount = reader.ReadUInt16(); //Number of Ligature tables
            ushort[] ligOffsets = reader.ReadUInt16Array(ligCount);
            //
            LigatureTable[] ligTables = ligSetTable.Ligatures = new LigatureTable[ligCount];
            for (int i = 0; i < ligCount; ++i)
            {
                ligTables[i] = ReadLigatureTable(reader, beginAt + ligOffsets[i]);
            }
            return ligSetTable;
        }

        private LigatureTable ReadLigatureTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            //
            ushort glyphIndex = reader.ReadUInt16();
            ushort compCount = reader.ReadUInt16();
            return new LigatureTable(glyphIndex, reader.ReadUInt16Array(compCount - 1));
        }

        private LookupSubTable ReadLookupType3(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16(); //The subtable has one format: AlternateSubstFormat1.
            switch (format)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort alternativeSetCount = reader.ReadUInt16();
                        ushort[] alternativeTableOffsets = reader.ReadUInt16Array(alternativeSetCount);

                        LkSubTableT3 subTable = new LkSubTableT3();
                        AlternativeSetTable[] alternativeSetTables = new AlternativeSetTable[alternativeSetCount];
                        subTable.AlternativeSetTables = alternativeSetTables;
                        for (int n = 0; n < alternativeSetCount; ++n)
                        {
                            alternativeSetTables[n] = ReadAlternativeSetTable(reader, subTableStartAt + alternativeTableOffsets[n]);
                        }
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);

                        return subTable;
                    }
            }
        }

        private AlternativeSetTable ReadAlternativeSetTable(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            // 
            AlternativeSetTable altTable = new AlternativeSetTable();
            ushort glyphCount = reader.ReadUInt16();
            altTable.alternativeGlyphIds = reader.ReadUInt16Array(glyphCount);
            return altTable;
        }

        private LookupSubTable ReadLookupType2(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    throw new NotSupportedException();
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort seqCount = reader.ReadUInt16();
                        ushort[] seqOffsets = reader.ReadUInt16Array(seqCount);

                        var subTable = new LkSubTableT2();
                        subTable.SeqTables = new SequenceTable[seqCount];
                        for (int n = 0; n < seqCount; ++n)
                        {
                            reader.BaseStream.Seek(subTableStartAt + seqOffsets[n], SeekOrigin.Begin);
                            ushort glyphCount = reader.ReadUInt16();
                            subTable.SeqTables[n] = new SequenceTable(
                                reader.ReadUInt16Array(glyphCount));
                        }
                        subTable.CoverageTable = CoverageConverter.Read(reader, subTableStartAt + coverageOffset);

                        return subTable;
                    }
            }
        }

        private LookupSubTable ReadLookupType1(EndianReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            ushort coverage = reader.ReadUInt16();
            switch (format)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        ushort deltaGlyph = reader.ReadUInt16();
                        var coverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return new LkSubTableT1Fmt1(coverageTable, deltaGlyph);
                    }
                case 2:
                    {
                        ushort glyphCount = reader.ReadUInt16();
                        ushort[] substituteGlyphs = reader.ReadUInt16Array(glyphCount); // 	Array of substitute GlyphIDs-ordered by Coverage Index                                 
                        var coverageTable = CoverageConverter.Read(reader, subTableStartAt + coverage);
                        return new LkSubTableT1Fmt2(coverageTable, substituteGlyphs);
                    }
            }
        }
    }
}
