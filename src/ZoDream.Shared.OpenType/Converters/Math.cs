using System;
using System.Data.Common;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MathConverter : TypefaceConverter<MathTable>
    {
        public override MathTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            long beginAt = reader.BaseStream.Position;
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            ushort mathConstants_offset = reader.ReadUInt16();
            ushort mathGlyphInfo_offset = reader.ReadUInt16();
            ushort mathVariants_offset = reader.ReadUInt16();
            var res = new MathTable();
            //(1)
            reader.BaseStream.Position = beginAt + mathConstants_offset;
            res.ConstTable = ReadMathConstantsTable(reader);
            
            //(2)
            reader.BaseStream.Position = beginAt + mathGlyphInfo_offset;
            long startAt = reader.BaseStream.Position;
            ushort offsetTo_MathItalicsCorrectionInfo_Table = reader.ReadUInt16();
            ushort offsetTo_MathTopAccentAttachment_Table = reader.ReadUInt16();
            ushort offsetTo_Extended_Shape_coverage_Table = reader.ReadUInt16();
            ushort offsetTo_MathKernInfo_Table = reader.ReadUInt16();
            //-----------------------

            //(2.1)
            reader.BaseStream.Position = startAt + offsetTo_MathItalicsCorrectionInfo_Table;
            res.ItalicCorrectionInfo = ReadMathItalicCorrectionInfoTable(reader);

            //(2.2)
            reader.BaseStream.Position = startAt + offsetTo_MathTopAccentAttachment_Table;
            res.TopAccentAttachmentTable = ReadMathTopAccentAttachment(reader);
            //


            //(2.3)
            if (offsetTo_Extended_Shape_coverage_Table > 0)
            {
                //may be null, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                res.ExtendedShapeCoverageTable = CoverageConverter.Read(reader, startAt + offsetTo_Extended_Shape_coverage_Table);
            }

            //(2.4)
            if (offsetTo_MathKernInfo_Table > 0)
            {
                //may be null, eg. latin-modern-math.otf => not found
                //we found this in Asana-math-regular
                reader.BaseStream.Position = startAt + offsetTo_MathKernInfo_Table;
                startAt = reader.BaseStream.Position;

                ushort mathKernCoverage_offset = reader.ReadUInt16();
                ushort mathKernCount = reader.ReadUInt16();

                ushort[] allKernRecOffset = reader.ReadArray(4 * mathKernCount, reader.ReadUInt16);//*** 

                //read each kern table  
                res.KernInfoRecords = new MathKernInfoRecord[mathKernCount];
                int index = 0;
                ushort m_kern_offset = 0;

                for (int i = 0; i < mathKernCount; ++i)
                {

                    //top-right
                    m_kern_offset = allKernRecOffset[index];

                    MathKern topRight = null, topLeft = null, bottomRight = null, bottomLeft = null;

                    if (m_kern_offset > 0)
                    {
                        reader.BaseStream.Position = beginAt + m_kern_offset;
                        topRight = ReadMathKernTable(reader);
                    }
                    //top-left
                    m_kern_offset = allKernRecOffset[index + 1];
                    if (m_kern_offset > 0)
                    {
                        reader.BaseStream.Position = beginAt + m_kern_offset;
                        topLeft = ReadMathKernTable(reader);
                    }
                    //bottom-right
                    m_kern_offset = allKernRecOffset[index + 2];
                    if (m_kern_offset > 0)
                    {
                        reader.BaseStream.Position = beginAt + m_kern_offset;
                        bottomRight = ReadMathKernTable(reader);
                    }
                    //bottom-left
                    m_kern_offset = allKernRecOffset[index + 3];
                    if (m_kern_offset > 0)
                    {
                        reader.BaseStream.Position = beginAt + m_kern_offset;
                        bottomLeft = ReadMathKernTable(reader);
                    }

                    res.KernInfoRecords[i] = new MathKernInfoRecord(topRight, topLeft, bottomRight, bottomLeft);

                    index += 4;//***
                }

                //-----
                res.KernInfoCoverage = CoverageConverter.Read(reader, beginAt + mathKernCoverage_offset);
            }
            //
            //(3)
            reader.BaseStream.Position = beginAt + mathVariants_offset;
            res.VariantsTable = ReadMathVariantsTable(reader);

            //NOTE: expose  MinConnectorOverlap via _mathConstTable
            res.ConstTable.MinConnectorOverlap = res.VariantsTable.MinConnectorOverlap;

            return res;
        }


        private MathTopAccentAttachmentTable ReadMathTopAccentAttachment(EndianReader reader)
        {
            long beginAt = reader.BaseStream.Position;
            var res = new MathTopAccentAttachmentTable();

            ushort coverageOffset = reader.ReadUInt16();
            ushort topAccentAttachmentCount = reader.ReadUInt16();
            res.TopAccentAttachment = reader.ReadArray(topAccentAttachmentCount, () => ReadMathValueRecord(reader));
            if (coverageOffset > 0)
            {
                //may be null?, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                res.CoverageTable = CoverageConverter.Read(reader, beginAt + coverageOffset);
            }

            return res;
        }

        private MathItalicsCorrectonInfoTable ReadMathItalicCorrectionInfoTable(EndianReader reader)
        {
            long beginAt = reader.BaseStream.Position;
            var res = new MathItalicsCorrectonInfoTable();
            //MathItalicsCorrectionInfo Table
            //Type           Name                           Description
            //Offset16       Coverage                       Offset to Coverage table - from the beginning of MathItalicsCorrectionInfo table.
            //uint16         ItalicsCorrectionCount         Number of italics correction values.Should coincide with the number of covered glyphs.
            //MathValueRecord ItalicsCorrection[ItalicsCorrectionCount]  Array of MathValueRecords defining italics correction values for each covered glyph. 
            ushort coverageOffset = reader.ReadUInt16();
            ushort italicCorrectionCount = reader.ReadUInt16();
            res.ItalicCorrections = reader.ReadArray(italicCorrectionCount, () => ReadMathValueRecord(reader));
            //read coverage ...
            if (coverageOffset > 0)
            {
                //may be null?, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                res.CoverageTable = CoverageConverter.Read(reader, beginAt + coverageOffset);
            }
            return res;
        }

        private void ReadMathKernInfoTable(EndianReader reader)
        {
            throw new NotImplementedException();
        }

        private MathVariantsTable ReadMathVariantsTable(EndianReader reader)
        {
            var res = new MathVariantsTable();

            long beginAt = reader.BaseStream.Position;
            //
            res.MinConnectorOverlap = reader.ReadUInt16();
            //
            ushort vertGlyphCoverageOffset = reader.ReadUInt16();
            ushort horizGlyphCoverageOffset = reader.ReadUInt16();
            ushort vertGlyphCount = reader.ReadUInt16();
            ushort horizGlyphCount = reader.ReadUInt16();
            ushort[] vertGlyphConstructions = reader.ReadArray(vertGlyphCount, reader.ReadUInt16);
            ushort[] horizonGlyphConstructions = reader.ReadArray(horizGlyphCount, reader.ReadUInt16);
            //

            if (vertGlyphCoverageOffset > 0)
            {
                res.vertCoverage = CoverageConverter.Read(reader, beginAt + vertGlyphCoverageOffset);
            }

            if (horizGlyphCoverageOffset > 0)
            {
                //may be null?, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                res.horizCoverage = CoverageConverter.Read(reader, beginAt + horizGlyphCoverageOffset);
            }

            //read math construction table

            //(3.1)
            //vertical
            var vertGlyphConstructionTables = res.vertConstructionTables = new MathGlyphConstruction[vertGlyphCount];
            for (int i = 0; i < vertGlyphCount; ++i)
            {
                reader.BaseStream.Position = beginAt + vertGlyphConstructions[i];
                vertGlyphConstructionTables[i] = ReadMathGlyphConstructionTable(reader);
            }

            //(3.2)
            //horizon
            var horizGlyphConstructionTables = res.horizConstructionTables = new MathGlyphConstruction[horizGlyphCount];
            for (int i = 0; i < horizGlyphCount; ++i)
            {
                reader.BaseStream.Position = beginAt + horizonGlyphConstructions[i];
                horizGlyphConstructionTables[i] = ReadMathGlyphConstructionTable(reader);
            }
            return res;
        }

        private MathGlyphConstruction ReadMathGlyphConstructionTable(EndianReader reader)
        {
            long beginAt = reader.BaseStream.Position;

            var glyphConstructionTable = new MathGlyphConstruction();

            ushort glyphAsmOffset = reader.ReadUInt16();
            ushort variantCount = reader.ReadUInt16();

            var variantRecords = glyphConstructionTable.glyphVariantRecords = new MathGlyphVariantRecord[variantCount];

            for (int i = 0; i < variantCount; ++i)
            {
                variantRecords[i] = new MathGlyphVariantRecord(
                  reader.ReadUInt16(),
                  reader.ReadUInt16()
                );
            }


            //read glyph asm table
            if (glyphAsmOffset > 0)//may be NULL
            {
                reader.BaseStream.Position = beginAt + glyphAsmOffset;
                FillGlyphAssemblyInfo(reader, glyphConstructionTable);
            }
            return glyphConstructionTable;
        }

        private void FillGlyphAssemblyInfo(EndianReader reader, 
            MathGlyphConstruction glyphConstruction)
        {
            glyphConstruction.GlyphAsm_ItalicCorrection = ReadMathValueRecord(reader);
            ushort partCount = reader.ReadUInt16();
            var partRecords = glyphConstruction.GlyphAsm_GlyphPartRecords = new GlyphPartRecord[partCount];
            for (int i = 0; i < partCount; ++i)
            {
                partRecords[i] = new GlyphPartRecord(
                  reader.ReadUInt16(),
                  reader.ReadUInt16(),
                  reader.ReadUInt16(),
                  reader.ReadUInt16(),
                  reader.ReadUInt16()
                );
            }
        }

        
        private MathConstants ReadMathConstantsTable(EndianReader reader)
        {
            var mc = new MathConstants();
            mc.ScriptPercentScaleDown = reader.ReadInt16();
            mc.ScriptScriptPercentScaleDown = reader.ReadInt16();
            mc.DelimitedSubFormulaMinHeight = reader.ReadUInt16();
            mc.DisplayOperatorMinHeight = reader.ReadUInt16();
            //
            //            

            mc.MathLeading = ReadMathValueRecord(reader);
            mc.AxisHeight = ReadMathValueRecord(reader);
            mc.AccentBaseHeight = ReadMathValueRecord(reader);
            mc.FlattenedAccentBaseHeight = ReadMathValueRecord(reader);

            // 
            mc.SubscriptShiftDown = ReadMathValueRecord(reader);
            mc.SubscriptTopMax = ReadMathValueRecord(reader);
            mc.SubscriptBaselineDropMin = ReadMathValueRecord(reader);

            //
            mc.SuperscriptShiftUp = ReadMathValueRecord(reader);
            mc.SuperscriptShiftUpCramped = ReadMathValueRecord(reader);
            mc.SuperscriptBottomMin = ReadMathValueRecord(reader);
            mc.SuperscriptBaselineDropMax = ReadMathValueRecord(reader);
            //
            mc.SubSuperscriptGapMin = ReadMathValueRecord(reader);
            mc.SuperscriptBottomMaxWithSubscript = ReadMathValueRecord(reader);
            mc.SpaceAfterScript = ReadMathValueRecord(reader);

            //
            mc.UpperLimitGapMin = ReadMathValueRecord(reader);
            mc.UpperLimitBaselineRiseMin = ReadMathValueRecord(reader);
            mc.LowerLimitGapMin = ReadMathValueRecord(reader);
            mc.LowerLimitBaselineDropMin = ReadMathValueRecord(reader);

            // 
            mc.StackTopShiftUp = ReadMathValueRecord(reader);
            mc.StackTopDisplayStyleShiftUp = ReadMathValueRecord(reader);
            mc.StackBottomShiftDown = ReadMathValueRecord(reader);
            mc.StackBottomDisplayStyleShiftDown = ReadMathValueRecord(reader);
            mc.StackGapMin = ReadMathValueRecord(reader);
            mc.StackDisplayStyleGapMin = ReadMathValueRecord(reader);

            //
            mc.StretchStackTopShiftUp = ReadMathValueRecord(reader);
            mc.StretchStackBottomShiftDown = ReadMathValueRecord(reader);
            mc.StretchStackGapAboveMin = ReadMathValueRecord(reader);
            mc.StretchStackGapBelowMin = ReadMathValueRecord(reader);

            // 
            mc.FractionNumeratorShiftUp = ReadMathValueRecord(reader);
            mc.FractionNumeratorDisplayStyleShiftUp = ReadMathValueRecord(reader);
            mc.FractionDenominatorShiftDown = ReadMathValueRecord(reader);
            mc.FractionDenominatorDisplayStyleShiftDown = ReadMathValueRecord(reader);
            mc.FractionNumeratorGapMin = ReadMathValueRecord(reader);
            mc.FractionNumDisplayStyleGapMin = ReadMathValueRecord(reader);
            mc.FractionRuleThickness = ReadMathValueRecord(reader);
            mc.FractionDenominatorGapMin = ReadMathValueRecord(reader);
            mc.FractionDenomDisplayStyleGapMin = ReadMathValueRecord(reader);

            // 
            mc.SkewedFractionHorizontalGap = ReadMathValueRecord(reader);
            mc.SkewedFractionVerticalGap = ReadMathValueRecord(reader);

            //
            mc.OverbarVerticalGap = ReadMathValueRecord(reader);
            mc.OverbarRuleThickness = ReadMathValueRecord(reader);
            mc.OverbarExtraAscender = ReadMathValueRecord(reader);

            //
            mc.UnderbarVerticalGap = ReadMathValueRecord(reader);
            mc.UnderbarRuleThickness = ReadMathValueRecord(reader);
            mc.UnderbarExtraDescender = ReadMathValueRecord(reader);

            //
            mc.RadicalVerticalGap = ReadMathValueRecord(reader);
            mc.RadicalDisplayStyleVerticalGap = ReadMathValueRecord(reader);
            mc.RadicalRuleThickness = ReadMathValueRecord(reader);
            mc.RadicalExtraAscender = ReadMathValueRecord(reader);
            mc.RadicalKernBeforeDegree = ReadMathValueRecord(reader);
            mc.RadicalKernAfterDegree = ReadMathValueRecord(reader);
            mc.RadicalDegreeBottomRaisePercent = reader.ReadInt16();
            
            return mc;
        }

        private MathKern ReadMathKernTable(EndianReader reader)
        {
            ushort heightCount = reader.ReadUInt16();
            return new MathKern(heightCount,
              reader.ReadArray(heightCount, () => ReadMathValueRecord(reader)),
              reader.ReadArray(heightCount + 1, () => ReadMathValueRecord(reader))
            );
        }

        private MathValueRecord ReadMathValueRecord(EndianReader reader)
        {
            return new MathValueRecord(reader.ReadInt16(), reader.ReadUInt16());
        }

        public override void Write(EndianWriter writer, MathTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
