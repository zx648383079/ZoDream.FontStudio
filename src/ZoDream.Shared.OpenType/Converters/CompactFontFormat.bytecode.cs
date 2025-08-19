using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.CompactFontFormat;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public partial class CompactFontFormatConverter
    {

        public static Bytecode ReadBytecode(EndianReader reader)
        {
            var res = new Bytecode();
            ReadNameIndex(reader, res);
            ReadTopDICTIndex(reader, res);
            ReadStringIndex(reader, res);
            ResolveTopDictInfo(reader, res);
            ReadGlobalSubrIndex(reader, res);

            ReadFDSelect(reader, res);
            ReadFDArray(reader, res);


            ReadPrivateDict(reader, res);


            ReadCharStringsIndex(reader, res);
            ReadCharsets(reader, res);
            ReadEncodings(reader, res);

            return res;
        }

        private static void ReadEncodings(EndianReader reader, Bytecode instance)
        {
            byte format = reader.ReadByte();
            switch (format)
            {
                default:
                    break;
                case 0:
                    ReadFormat0Encoding(reader, instance);
                    break;
                case 1:
                    ReadFormat1Encoding(reader, instance);
                    break;

            }
        }

        private static void ReadFormat0Encoding(EndianReader reader, Bytecode instance)
        {
            throw new NotImplementedException();
        }

        private static void ReadFormat1Encoding(EndianReader reader, Bytecode instance)
        {
            throw new NotImplementedException();
        }

        private static void ReadCharsets(EndianReader reader, Bytecode instance)
        {
            reader.BaseStream.Position = instance.CffStartAt + instance.CharsetOffset;
            //TODO: ...
            byte format = reader.ReadByte();
            switch (format)
            {
                default: throw new NotSupportedException();
                case 0:
                    ReadCharsetsFormat0(reader, instance);
                    break;
                case 1:
                    ReadCharsetsFormat1(reader, instance);
                    break;
                case 2:
                    ReadCharsetsFormat2(reader, instance);
                    break;
            }
        }

        private static void ReadCharsetsFormat2(EndianReader reader, Bytecode instance)
        {
            var cff1Glyphs = instance.CurrentFont.Glyphs;
            int nGlyphs = cff1Glyphs.Length;
            for (int i = 1; i < nGlyphs;)
            {
                int sid = reader.ReadUInt16();// First glyph in range 
                int count = reader.ReadUInt16() + 1;//since it not include first elem
                do
                {
                    var d = cff1Glyphs[i].Glyph;
                    d.Name = GetSid(d.SIDName = (ushort)sid);

                    count--;
                    i++;
                    sid++;
                } while (count > 0);
            }
        }

        private static void ReadCharsetsFormat1(EndianReader reader, Bytecode instance)
        {
            var cff1Glyphs = instance.CurrentFont.Glyphs;
            int nGlyphs = cff1Glyphs.Length;
            for (int i = 1; i < nGlyphs;)
            {
                int sid = reader.ReadUInt16();// First glyph in range 
                int count = reader.ReadByte() + 1;//since it not include first elem
                do
                {
                    var d = cff1Glyphs[i].Glyph;
                    d.Name = GetSid(d.SIDName = (ushort)sid);

                    count--;
                    i++;
                    sid++;
                } while (count > 0);
            }
        }

        private static void ReadCharsetsFormat0(EndianReader reader, Bytecode instance)
        {
            var cff1Glyphs = instance.CurrentFont.Glyphs;
            int nGlyphs = cff1Glyphs.Length;
            for (int i = 1; i < nGlyphs; ++i)
            {
                var d = cff1Glyphs[i].Glyph;
                d.Name = GetSid(d.SIDName = reader.ReadUInt16());
            }
        }

        private static void ReadCharStringsIndex(EndianReader reader, Bytecode instance)
        {
            reader.Position = instance.CffStartAt + instance.CharStringsOffset;
            var offsets = ReadIndexDataOffset(reader);

            int glyphCount = offsets.Length;
            //assume Type2
            //TODO: review here  

            var glyphs = new GlyphData[glyphCount];
            instance.CurrentFont.Glyphs = glyphs;
            var type2Parser = new Type2CharStringParser();
            type2Parser.SetCurrentCff1Font(instance.CurrentFont);


            //cid font or not

            var fdRangeProvider = new FDRangeProvider(instance.CidFontInfo.fdRanges);
            bool isCidFont = instance.CidFontInfo.fdRanges != null;

            for (int i = 0; i < glyphCount; ++i)
            {
                var offset = offsets[i];
                var buffer = reader.ReadAsStream(offset.Value);
                //now we can parse the raw glyph instructions 

                var glyphData = new FontGlyphData();

                if (isCidFont)
                {
                    //select  proper local private dict 
                    fdRangeProvider.SetCurrentGlyphIndex((ushort)i);
                    type2Parser.SetCidFontDict(instance.CurrentFont._cidFontDict[fdRangeProvider.SelectedFDArray]);
                }

                var instList = type2Parser.ParseType2CharString(buffer);
                if (instList != null)
                {
                    //use compact form or not

                    if (instance.UseCompactInstruction)
                    {
                        //this is our extension,
                        //if you don't want compact version
                        //just use original 

                        glyphData.GlyphInstructions = instance.InstCompacter.Compact(instList.InnerInsts);


                    }
                    else
                    {
                        glyphData.GlyphInstructions = instList.InnerInsts.ToArray();

                    }
                }
                glyphs[i] = new GlyphData(glyphData, (ushort)i);
            }
        }

        private static void ReadPrivateDict(EndianReader reader, Bytecode instance)
        {
            if (instance.PrivateDICTLen == 0) 
            {
                return;
            }
            reader.Position = instance.CffStartAt + instance.PrivateDICTOffset;
            var dicData = ReadDICTData(reader, instance.PrivateDICTLen);

            if (dicData.Length > 0)
            {
                //interpret the values of private dict 
                foreach (var dicEntry in dicData)
                {
                    switch (dicEntry.Operator.Name)
                    {
                        case "Subrs":
                            {
                                int localSubrsOffset = (int)dicEntry.Operands[0].RealNumValue;
                                reader.BaseStream.Position = instance.CffStartAt + instance.PrivateDICTOffset + localSubrsOffset;
                                ReadLocalSubrs(reader, instance);
                            }
                            break;
                        case "defaultWidthX":
                            instance.CurrentFont._defaultWidthX = (int)dicEntry.Operands[0].RealNumValue;
                            break;
                        case "nominalWidthX":
                            instance.CurrentFont._nominalWidthX = (int)dicEntry.Operands[0].RealNumValue;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void ReadLocalSubrs(EndianReader reader, Bytecode instance)
        {
            instance.CurrentFont._localSubrRawBufferList = ReadSubrBuffer(reader);
        }

        private static void ReadFDArray(EndianReader reader, Bytecode instance)
        {
            if (instance.CidFontInfo.FDArray == 0) 
            { 
                return; 
            }
            reader.Position = instance.CffStartAt + instance.CidFontInfo.FDArray;

            var offsets = ReadIndexDataOffset(reader);

            var fontDicts = new List<FontDict>();
            instance.CurrentFont._cidFontDict = fontDicts;

            for (int i = 0; i < offsets.Length; ++i)
            {
                //read DICT data 
                var dic = ReadDICTData(reader, offsets[i].Value);
                //translate

                int offset = 0;
                int size = 0;
                int name = 0;

                foreach (var entry in dic)
                {
                    switch (entry.Operator.Name)
                    {
                        default: throw new NotSupportedException();
                        case "FontName":
                            name = (int)entry.Operands[0].RealNumValue;
                            break;
                        case "Private": //private dic
                            size = (int)entry.Operands[0].RealNumValue;
                            offset = (int)entry.Operands[1].RealNumValue;
                            break;
                    }
                }

                var fontdict = new FontDict(size, offset);
                fontdict.FontName = name;
                fontDicts.Add(fontdict);
            }
            //-----------------

            foreach (var fdict in fontDicts)
            {
                reader.Position = instance.CffStartAt + fdict.PrivateDicOffset;

                var dicData = ReadDICTData(reader, fdict.PrivateDicSize);

                if (dicData.Length > 0)
                {
                    //interpret the values of private dict 
                    foreach (var dicEntry in dicData)
                    {
                        switch (dicEntry.Operator.Name)
                        {
                            case "Subrs":
                                {
                                    int localSubrsOffset = (int)dicEntry.Operands[0].RealNumValue;
                                    reader.Position = instance.CffStartAt + fdict.PrivateDicOffset + localSubrsOffset;
                                    fdict.LocalSubr = ReadSubrBuffer(reader);
                                }
                                break;
                            case "defaultWidthX":

                                break;
                            case "nominalWidthX":

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private static List<Stream> ReadSubrBuffer(EndianReader reader)
        {
            var offsets = ReadIndexDataOffset(reader);
            if (offsets == null)
            {
                return [];
            }
            int nsubrs = offsets.Length;
            var rawBufferList = new List<Stream>();

            for (int i = 0; i < nsubrs; ++i)
            {
                rawBufferList.Add(reader.ReadAsStream(offsets[i].Value));
            }
            return rawBufferList;
        }

        private static void ReadGlobalSubrIndex(EndianReader reader, Bytecode instance)
        {
            foreach (var entry in instance.TopDic)
            {
                switch (entry.Operator.Name)
                {
                    default:
                        break;
                    case "XUID": break;//nothing
                    case "version":
                        instance.CurrentFont.Version = GetSid((int)entry.Operands[0].RealNumValue);
                        break;
                    case "Notice":
                        instance.CurrentFont.Notice = GetSid((int)entry.Operands[0].RealNumValue);
                        break;
                    case "Copyright":
                        instance.CurrentFont.CopyRight = GetSid((int)entry.Operands[0].RealNumValue);
                        break;
                    case "FullName":
                        instance.CurrentFont.FullName = GetSid((int)entry.Operands[0].RealNumValue);
                        break;
                    case "FamilyName":
                        instance.CurrentFont.FamilyName = GetSid((int)entry.Operands[0].RealNumValue);
                        break;
                    case "Weight":
                        instance.CurrentFont.Weight = GetSid((int)entry.Operands[0].RealNumValue);
                        break;
                    case "UnderlinePosition":
                        instance.CurrentFont.UnderlinePosition = entry.Operands[0].RealNumValue;
                        break;
                    case "UnderlineThickness":
                        instance.CurrentFont.UnderlineThickness = entry.Operands[0].RealNumValue;
                        break;
                    case "FontBBox":
                        instance.CurrentFont.FontBBox = new double[] {
                            entry.Operands[0].RealNumValue,
                            entry.Operands[1].RealNumValue,
                            entry.Operands[2].RealNumValue,
                            entry.Operands[3].RealNumValue};
                        break;
                    case "CharStrings":
                        instance.CharStringsOffset = (int)entry.Operands[0].RealNumValue;
                        break;
                    case "charset":
                        instance.CharsetOffset = (int)entry.Operands[0].RealNumValue;
                        break;
                    case "Encoding":
                        instance.EncodingOffset = (int)entry.Operands[0].RealNumValue;
                        break;
                    case "Private":
                        //private DICT size and offset
                        instance.PrivateDICTLen = (int)entry.Operands[0].RealNumValue;
                        instance.PrivateDICTOffset = (int)entry.Operands[1].RealNumValue;
                        break;
                    case "ROS":
                        //http://wwwimages.adobe.com/www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5176.CFF.pdf
                        //A CFF CIDFont has the CIDFontName in the Name INDEX and a corresponding Top DICT. 
                        //The Top DICT begins with ROS operator which specifies the Registry-Ordering - Supplement for the font.
                        //This will indicate to a CFF parser that special CID processing should be applied to this font. Specifically:

                        //ROS operator combines the Registry, Ordering, and Supplement keys together.

                        //see Adobe Cmap resource , https://github.com/adobe-type-tools/cmap-resources

                        instance.CidFontInfo.ROS_Register = GetSid((int)entry.Operands[0].RealNumValue);
                        instance.CidFontInfo.ROS_Ordering = GetSid((int)entry.Operands[1].RealNumValue);
                        instance.CidFontInfo.ROS_Supplement = GetSid((int)entry.Operands[2].RealNumValue);

                        break;
                    case "CIDFontVersion":
                        instance.CidFontInfo.CIDFontVersion = entry.Operands[0].RealNumValue;
                        break;
                    case "CIDCount":
                        instance.CidFontInfo.CIDFountCount = (int)entry.Operands[0].RealNumValue;
                        break;
                    case "FDSelect":
                        instance.CidFontInfo.FDSelect = (int)entry.Operands[0].RealNumValue;
                        break;
                    case "FDArray":
                        instance.CidFontInfo.FDArray = (int)entry.Operands[0].RealNumValue;
                        break;
                }
            }
        }

        private static string GetSid(int realNumValue)
        {
            throw new NotImplementedException();
        }

        private static void ReadFDSelect(EndianReader reader, Bytecode instance)
        {
            if (instance.CidFontInfo.FDSelect == 0) 
            { 
                return; 
            }
            reader.Position = instance.CffStartAt + instance.CidFontInfo.FDSelect;
            byte format = reader.ReadByte();

            switch (format)
            {
                default:
                    throw new NotSupportedException();
                case 3:
                    {
                        ushort nRanges = reader.ReadUInt16();
                        FDRange3[] ranges = new FDRange3[nRanges + 1];

                        instance.CidFontInfo.fdSelectFormat = 3;
                        instance.CidFontInfo.fdRanges = ranges;
                        for (int i = 0; i < nRanges; ++i)
                        {
                            ranges[i] = new FDRange3(reader.ReadUInt16(), reader.ReadByte());
                        }

                        //end with //sentinel
                        ranges[nRanges] = new FDRange3(reader.ReadUInt16(), 0);//sentinel

                    }
                    break;
            }
        }

        private static void ResolveTopDictInfo(EndianReader reader, Bytecode instance)
        {
            throw new NotImplementedException();
        }

        private static void ReadStringIndex(EndianReader reader, Bytecode instance)
        {
            var offsets = ReadIndexDataOffset(reader);
            if (offsets.Length == 0)
            {
                return;
            }
            var data = new string[offsets.Length];

            for (int i = 0; i < offsets.Length; ++i)
            {
                data[i] = reader.ReadString(offsets[i].Value);
            }
            instance.FontSet.UniqueStringTable = data;
        }

        private static void ReadTopDICTIndex(EndianReader reader, Bytecode instance)
        {
            var offsets = ReadIndexDataOffset(reader);
            int count = offsets.Length;
            if (count > 1)
            {
                //temp...
                //TODO: review here again
                throw new NotSupportedException();
            }
            for (int i = 0; i < count; ++i)
            {
                //read DICT data

                instance.TopDic = ReadDICTData(reader, offsets[i].Value);
            }
        }

        private static DataDicEntry[] ReadDICTData(EndianReader reader, int len)
        {
            int endBefore = (int)(reader.Position + len);
            var dicData = new List<DataDicEntry>();
            while (reader.Position < endBefore)
            {
                var dicEntry = ReadEntry(reader);
                dicData.Add(dicEntry);
            }
            return dicData.ToArray();
        }

        private static DataDicEntry ReadEntry(EndianReader reader)
        {
            var dicEntry = new DataDicEntry();
            var operands = new List<OperandCode>();

            while (true)
            {
                byte b0 = reader.ReadByte();

                if (b0 >= 0 && b0 <= 21)
                {
                    //operators
                    dicEntry.Operator = ReadOperator(reader, b0);
                    break; //**break after found operator
                }
                else if (b0 == 28 || b0 == 29)
                {
                    int num = ReadIntegerNumber(reader, b0);
                    operands.Add(new OperandCode(num, OperandKind.IntNumber));
                }
                else if (b0 == 30)
                {
                    double num = ReadRealNumber(reader);
                    operands.Add(new OperandCode(num, OperandKind.RealNumber));
                }
                else if (b0 >= 32 && b0 <= 254)
                {
                    int num = ReadIntegerNumber(reader, b0);
                    operands.Add(new OperandCode(num, OperandKind.IntNumber));
                }
                else
                {
                    throw new NotSupportedException("invalid DICT data b0 byte: " + b0);
                }
            }

            dicEntry.Operands = operands.ToArray();
            return dicEntry;
        }

        private static double ReadRealNumber(EndianReader reader)
        {
            var sb = new StringBuilder();

            bool done = false;
            bool exponentMissing = false;
            while (!done)
            {
                int b = reader.ReadByte();

                int nb_0 = (b >> 4) & 0xf;
                int nb_1 = (b) & 0xf;

                for (int i = 0; !done && i < 2; ++i)
                {
                    int nibble = (i == 0) ? nb_0 : nb_1;

                    switch (nibble)
                    {
                        case 0x0:
                        case 0x1:
                        case 0x2:
                        case 0x3:
                        case 0x4:
                        case 0x5:
                        case 0x6:
                        case 0x7:
                        case 0x8:
                        case 0x9:
                            sb.Append(nibble);
                            exponentMissing = false;
                            break;
                        case 0xa:
                            sb.Append('.');
                            break;
                        case 0xb:
                            sb.Append('E');
                            exponentMissing = true;
                            break;
                        case 0xc:
                            sb.Append("E-");
                            exponentMissing = true;
                            break;
                        case 0xd:
                            break;
                        case 0xe:
                            sb.Append('-');
                            break;
                        case 0xf:
                            done = true;
                            break;
                        default:
                            throw new Exception("IllegalArgumentException");
                    }
                }
            }
            if (exponentMissing)
            {
                // the exponent is missing, just append "0" to avoid an exception
                // not sure if 0 is the correct value, but it seems to fit
                // see PDFBOX-1522
                sb.Append('0');
            }
            if (sb.Length == 0)
            {
                return 0d;
            }


            //TODO: use TryParse 

            if (!double.TryParse(sb.ToString(),
                System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                System.Globalization.CultureInfo.InvariantCulture, out double value))
            {
                throw new NotSupportedException();
            }
            return value;
        }

        private static int ReadIntegerNumber(EndianReader reader, byte b0)
        {
            if (b0 == 28)
            {
                return reader.ReadInt16();
            }
            else if (b0 == 29)
            {
                return reader.ReadInt32();
            }
            else if (b0 >= 32 && b0 <= 246)
            {
                return b0 - 139;
            }
            else if (b0 >= 247 && b0 <= 250)
            {
                int b1 = reader.ReadByte();
                return (b0 - 247) * 256 + b1 + 108;
            }
            else if (b0 >= 251 && b0 <= 254)
            {
                int b1 = reader.ReadByte();
                return -(b0 - 251) * 256 - b1 - 108;
            }
            else
            {
                throw new Exception();
            }
        }

        private static Operator ReadOperator(EndianReader reader, byte b0)
        {
            byte b1 = 0;
            if (b0 == 12)
            {
                //2 bytes
                b1 = reader.ReadByte();
            }
            return GetOperatorByKey(b0, b1);
        }

        private static void ReadNameIndex(EndianReader reader, Bytecode instance)
        {
            var items = ReadIndexDataOffset(reader);
            var nameItems = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                nameItems[i] = reader.ReadString(items[i].Value);
            }
            instance.FontSet.FontNames = nameItems;
            Debug.Assert(nameItems.Length > 0);
            instance.CurrentFont = new FontFamily()
            {
                FontName = nameItems[0]
            };
            instance.FontSet.Fonts.Add(instance.CurrentFont);
        }

        private static KeyValuePair<int, int>[] ReadIndexDataOffset(EndianReader reader)
        {
            var count = reader.ReadUInt16();
            if (count == 0)
            {
                return [];
            }
            int offSize = reader.ReadByte();
            var offsets = new int[count + 1];
            for (int i = 0; i <= count; ++i)
            {
                offsets[i] = reader.ReadInt32(offSize);
            }
            var res = new KeyValuePair<int, int>[count];
            for (int i = 0; i < count; ++i)
            {
                res[i] = new KeyValuePair<int, int>(offsets[i], offsets[i + 1] - offsets[i]);
            }
            return res;
        }


    }
}
