using System;
using System.Diagnostics;
using System.IO;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class Type2CharStringParser
    {
        int _hintStemCount = 0;
        bool _foundSomeStem = false;
        bool _enterPathConstructionSeq = false;

        Type2GlyphInstructionList _insts;
        int _current_integer_count = 0;
        bool _doStemCount = true;
        FontFamily _currentCff1Font;
        int _globalSubrBias;
        int _localSubrBias;
        FontDict _currentFontDict;

        Operand _latestOpName = Operand.Unknown;

        public void SetCurrentCff1Font(FontFamily currentCff1Font)
        {
            //this will provide subr buffer for callsubr callgsubr
            _currentFontDict = null;//reset
            _currentCff1Font = currentCff1Font;

            if (_currentCff1Font._globalSubrRawBufferList != null)
            {
                _globalSubrBias = CalculateBias(currentCff1Font._globalSubrRawBufferList.Count);
            }
            if (_currentCff1Font._localSubrRawBufferList != null)
            {
                _localSubrBias = CalculateBias(currentCff1Font._localSubrRawBufferList.Count);
            }
        }

        static int CalculateBias(int nsubr)
        {
            //-------------
            //from Technical Note #5176 (CFF spec)
            //resolve with bias
            //Card16 bias;
            //Card16 nSubrs = subrINDEX.count;
            //if (CharstringType == 1)
            //    bias = 0;
            //else if (nSubrs < 1240)
            //    bias = 107;
            //else if (nSubrs < 33900)
            //    bias = 1131;
            //else
            //    bias = 32768; 
            //find local subroutine 
            return (nsubr < 1240) ? 107 : (nsubr < 33900) ? 1131 : 32768;
        }

        public void SetCidFontDict(FontDict fontdic)
        {

            _currentFontDict = fontdic;
            if (fontdic.LocalSubr != null)
            {
                _localSubrBias = CalculateBias(_currentFontDict.LocalSubr.Count);
            }
            else
            {
                _localSubrBias = 0;
            }
        }

        void ParseType2CharStringBuffer(Stream input)
        {
            byte b0 = 0;

            bool cont = true;

            var reader = new EndianReader(input, EndianType.BigEndian);
            while (cont && reader.RemainingLength > 0)
            {
                b0 = reader.ReadByte();
                switch (b0)
                {
                    default: //else 32 -255
                        {
                            if (b0 < 32)
                            {
                                Debug.WriteLine("err!:" + b0);
                                return;
                            }
                            //
                            _insts.AddInt(ReadIntegerNumber(reader, b0));
                            if (_doStemCount)
                            {
                                _current_integer_count++;
                            }
                        }
                        break;
                    case 255:
                        {

                            //from https://www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5177.Type2.pdf
                            //If the charstring byte contains the value 255,
                            //the next four bytes indicate a two’s complement signed number.

                            //The first of these four bytes contains the highest order bits,
                            //he second byte contains the next higher order bits and
                            //the fourth byte contains the lowest order bits.

                            //eg. found in font Asana Math regular, glyph_index: 114 , 292, 1070 etc.

                            _insts.AddFloat(reader.ReadInt32());

                            if (_doStemCount)
                            {
                                _current_integer_count++;
                            }
                        }
                        break;
                    case (byte)Type2Operator1.shortint: // 28

                        //shortint
                        //First byte of a 3-byte sequence specifying a number.
                        //a ShortInt value is specified by using the operator (28) followed by two bytes
                        //which represent numbers between –32768 and + 32767.The
                        //most significant byte follows the(28)
                        byte s_b0 = reader.ReadByte();
                        byte s_b1 = reader.ReadByte();
                        _insts.AddInt((short)((s_b0 << 8) | (s_b1)));
                        //
                        if (_doStemCount)
                        {
                            _current_integer_count++;
                        }
                        break;
                    //---------------------------------------------------
                    case (byte)Type2Operator1._Reserved0_://???
                    case (byte)Type2Operator1._Reserved2_://???
                    case (byte)Type2Operator1._Reserved9_://???
                    case (byte)Type2Operator1._Reserved13_://???
                    case (byte)Type2Operator1._Reserved15_://???
                    case (byte)Type2Operator1._Reserved16_: //???
                    case (byte)Type2Operator1._Reserved17_: //???
                        //reserved, do nothing ?
                        break;

                    case (byte)Type2Operator1.escape: //12
                        {

                            b0 = reader.ReadByte();
                            switch ((Type2Operator2)b0)
                            {
                                default:
                                    if (b0 <= 38)
                                    {
                                        Debug.WriteLine("err!:" + b0);
                                        return;
                                    }
                                    break;
                                //-------------------------
                                //4.1: Path Construction Operators
                                case Type2Operator2.flex: _insts.AddOp(Operand.flex); break;
                                case Type2Operator2.hflex: _insts.AddOp(Operand.hflex); break;
                                case Type2Operator2.hflex1: _insts.AddOp(Operand.hflex1); break;
                                case Type2Operator2.flex1: _insts.AddOp(Operand.flex1); ; break;
                                //-------------------------
                                //4.4: Arithmetic Operators
                                case Type2Operator2.abs: _insts.AddOp(Operand.abs); break;
                                case Type2Operator2.add: _insts.AddOp(Operand.add); break;
                                case Type2Operator2.sub: _insts.AddOp(Operand.sub); break;
                                case Type2Operator2.div: _insts.AddOp(Operand.div); break;
                                case Type2Operator2.neg: _insts.AddOp(Operand.neg); break;
                                case Type2Operator2.random: _insts.AddOp(Operand.random); break;
                                case Type2Operator2.mul: _insts.AddOp(Operand.mul); break;
                                case Type2Operator2.sqrt: _insts.AddOp(Operand.sqrt); break;
                                case Type2Operator2.drop: _insts.AddOp(Operand.drop); break;
                                case Type2Operator2.exch: _insts.AddOp(Operand.exch); break;
                                case Type2Operator2.index: _insts.AddOp(Operand.index); break;
                                case Type2Operator2.roll: _insts.AddOp(Operand.roll); break;
                                case Type2Operator2.dup: _insts.AddOp(Operand.dup); break;

                                //-------------------------
                                //4.5: Storage Operators 
                                case Type2Operator2.put: _insts.AddOp(Operand.put); break;
                                case Type2Operator2.get: _insts.AddOp(Operand.get); break;
                                //-------------------------
                                //4.6: Conditional
                                case Type2Operator2.and: _insts.AddOp(Operand.and); break;
                                case Type2Operator2.or: _insts.AddOp(Operand.or); break;
                                case Type2Operator2.not: _insts.AddOp(Operand.not); break;
                                case Type2Operator2.eq: _insts.AddOp(Operand.eq); break;
                                case Type2Operator2.ifelse: _insts.AddOp(Operand.ifelse); break;
                            }

                            StopStemCount();
                        }
                        break;

                    //---------------------------------------------------------------------------
                    case (byte)Type2Operator1.endchar:
                        AddEndCharOp();
                        cont = false;
                        //when we found end char
                        //stop reading this...
                        break;
                    case (byte)Type2Operator1.rmoveto: AddMoveToOp(Operand.rmoveto); StopStemCount(); break;
                    case (byte)Type2Operator1.hmoveto: AddMoveToOp(Operand.hmoveto); StopStemCount(); break;
                    case (byte)Type2Operator1.vmoveto: AddMoveToOp(Operand.vmoveto); StopStemCount(); break;
                    //---------------------------------------------------------------------------
                    case (byte)Type2Operator1.rlineto: _insts.AddOp(Operand.rlineto); StopStemCount(); break;
                    case (byte)Type2Operator1.hlineto: _insts.AddOp(Operand.hlineto); StopStemCount(); break;
                    case (byte)Type2Operator1.vlineto: _insts.AddOp(Operand.vlineto); StopStemCount(); break;
                    case (byte)Type2Operator1.rrcurveto: _insts.AddOp(Operand.rrcurveto); StopStemCount(); break;
                    case (byte)Type2Operator1.hhcurveto: _insts.AddOp(Operand.hhcurveto); StopStemCount(); break;
                    case (byte)Type2Operator1.hvcurveto: _insts.AddOp(Operand.hvcurveto); StopStemCount(); break;
                    case (byte)Type2Operator1.rcurveline: _insts.AddOp(Operand.rcurveline); StopStemCount(); break;
                    case (byte)Type2Operator1.rlinecurve: _insts.AddOp(Operand.rlinecurve); StopStemCount(); break;
                    case (byte)Type2Operator1.vhcurveto: _insts.AddOp(Operand.vhcurveto); StopStemCount(); break;
                    case (byte)Type2Operator1.vvcurveto: _insts.AddOp(Operand.vvcurveto); StopStemCount(); break;
                    //-------------------------------------------------------------------
                    //4.3 Hint Operators
                    case (byte)Type2Operator1.hstem: AddStemToList(Operand.hstem); break;
                    case (byte)Type2Operator1.vstem: AddStemToList(Operand.vstem); break;
                    case (byte)Type2Operator1.vstemhm: AddStemToList(Operand.vstemhm); break;
                    case (byte)Type2Operator1.hstemhm: AddStemToList(Operand.hstemhm); break;
                    //-------------------------------------------------------------------
                    case (byte)Type2Operator1.hintmask: AddHintMaskToList(reader); StopStemCount(); break;
                    case (byte)Type2Operator1.cntrmask: AddCounterMaskToList(reader); StopStemCount(); break;
                    //-------------------------------------------------------------------
                    //4.7: Subroutine Operators                   
                    case (byte)Type2Operator1._return:
                        return;
                    //-------------------------------------------------------------------
                    case (byte)Type2Operator1.callsubr:
                        {
                            //get local subr proc
                            if (_currentCff1Font != null)
                            {
                                Type2Instruction inst = _insts.RemoveLast();
                                if (!inst.IsLoadInt)
                                {
                                    throw new NotSupportedException();
                                }
                                if (_doStemCount)
                                {
                                    _current_integer_count--;
                                }
                                //subr_no must be adjusted with proper bias value 
                                if (_currentCff1Font._localSubrRawBufferList != null)
                                {
                                    ParseType2CharStringBuffer(_currentCff1Font._localSubrRawBufferList[inst.Value + _localSubrBias]);
                                }
                                else if (_currentFontDict != null)
                                {
                                    //use private dict
                                    ParseType2CharStringBuffer(_currentFontDict.LocalSubr[inst.Value + _localSubrBias]);
                                }
                                else
                                {
                                    throw new NotSupportedException();
                                }
                            }
                        }
                        break;
                    case (byte)Type2Operator1.callgsubr:
                        {
                            if (_currentCff1Font != null)
                            {
                                Type2Instruction inst = _insts.RemoveLast();
                                if (!inst.IsLoadInt)
                                {
                                    throw new NotSupportedException();
                                }
                                if (_doStemCount)
                                {
                                    _current_integer_count--;
                                }
                                //subr_no must be adjusted with proper bias value 
                                //load global subr
                                ParseType2CharStringBuffer(_currentCff1Font._globalSubrRawBufferList[inst.Value + _globalSubrBias]);
                            }
                        }
                        break;
                }
            }
        }

        public Type2GlyphInstructionList ParseType2CharString(Stream input)
        {
            //reset
            _hintStemCount = 0;
            _current_integer_count = 0;
            _foundSomeStem = false;
            _enterPathConstructionSeq = false;
            _doStemCount = true;

            _insts = new Type2GlyphInstructionList();
            ParseType2CharStringBuffer(input);

            return _insts;
        }

        void StopStemCount()
        {
            _current_integer_count = 0;
            _doStemCount = false;
        }

        void AddEndCharOp()
        {
            //from https://www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5177.Type2.pdf
            //Note 4 The first stack - clearing operator, which must be one of
            //hstem, hstemhm, vstem, vstemhm, 
            //cntrmask, hintmask, 
            //hmoveto, vmoveto, rmoveto,
            //or endchar,
            //takes an additional argument — the width(as described earlier), which may be expressed as zero or one numeric argument

            if (!_foundSomeStem && !_enterPathConstructionSeq)
            {
                if (_insts.Count > 0)
                {
                    _insts.ChangeFirstInstToGlyphWidthValue();
                }
            }
            //takes an additional argument — the width(as described earlier), which may be expressed as zero or one numeric argument
            _insts.AddOp(Operand.endchar);
        }



        /// <summary>
        /// for hmoveto, vmoveto, rmoveto
        /// </summary>
        /// <param name="op"></param>
        void AddMoveToOp(Operand op)
        {
            //from https://www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5177.Type2.pdf
            //Note 4 The first stack - clearing operator, which must be one of
            //hstem, hstemhm, vstem, vstemhm, 
            //cntrmask, hintmask, 
            //hmoveto, vmoveto, rmoveto,
            //or endchar,
            //takes an additional argument — the width(as described earlier), which may be expressed as zero or one numeric argument 
            //just add

            if (!_foundSomeStem && !_enterPathConstructionSeq)
            {
                if (op == Operand.rmoveto)
                {
                    if ((_insts.Count % 2) != 0)
                    {
                        _insts.ChangeFirstInstToGlyphWidthValue();
                    }
                }
                else
                {
                    //vmoveto, hmoveto
                    if (_insts.Count > 1)
                    {
                        //...
                        _insts.ChangeFirstInstToGlyphWidthValue();
                    }
                }
            }
            _enterPathConstructionSeq = true;
            _insts.AddOp(op);
        }
        /// <summary>
        /// for hstem, hstemhm, vstem, vstemhm
        /// </summary>
        /// <param name="stemName"></param>
        void AddStemToList(Operand stemName)
        {

            //from https://www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5177.Type2.pdf
            //Note 4 The first stack - clearing operator, which must be one of
            //hstem, hstemhm, vstem, vstemhm, 
            //cntrmask, hintmask, 
            //hmoveto, vmoveto, rmoveto,
            //or endchar,
            //takes an additional argument — the width(as described earlier), which may be expressed as zero or one numeric argument

            //support 4 kinds 

            //1. 
            //|- y dy {dya dyb}*  hstemhm (18) |-
            //2.
            //|- x dx {dxa dxb}* vstemhm (23) |-
            //3.
            //|- y dy {dya dyb}*  hstem (1) |-
            //4. 
            //|- x dx {dxa dxb}*  vstem (3) |- 
            //-----------------------

            //notes
            //The sequence and form of a Type 2 charstring program may be
            //represented as:
            //w? { hs* vs*cm * hm * mt subpath}? { mt subpath} *endchar 

            if ((_current_integer_count % 2) != 0)
            {
                //all kind has even number of stem
                if (_foundSomeStem)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    //the first one is 'width'
                    _insts.ChangeFirstInstToGlyphWidthValue();
                    _current_integer_count--;
                }
            }
            _hintStemCount += (_current_integer_count / 2); //save a snapshot of stem count
            _insts.AddOp(stemName);
            _current_integer_count = 0;//clear
            _foundSomeStem = true;
            _latestOpName = stemName;
        }
        /// <summary>
        /// add hintmask
        /// </summary>
        /// <param name="reader"></param>
        void AddHintMaskToList(EndianReader reader)
        {
            if (_foundSomeStem && _current_integer_count > 0)
            {

                //type2 5177.pdf
                //...
                //If hstem and vstem hints are both declared at the beginning of
                //a charstring, and this sequence is followed directly by the
                //hintmask or cntrmask operators, ...
                //the vstem hint operator need not be included ***

#if DEBUG
                if ((_current_integer_count % 2) != 0)
                {
                    throw new NotSupportedException();
                }
                else
                {

                }
#endif
                if (_doStemCount)
                {
                    switch (_latestOpName)
                    {
                        case Operand.hstem:
                            //add vstem  ***( from reason above)

                            _hintStemCount += (_current_integer_count / 2); //save a snapshot of stem count
                            _insts.AddOp(Operand.vstem);
                            _latestOpName = Operand.vstem;
                            _current_integer_count = 0; //clear
                            break;
                        case Operand.hstemhm:
                            //add vstem  ***( from reason above) ??
                            _hintStemCount += (_current_integer_count / 2); //save a snapshot of stem count
                            _insts.AddOp(Operand.vstem);
                            _latestOpName = Operand.vstem;
                            _current_integer_count = 0;//clear
                            break;
                        case Operand.vstemhm:
                            //-------
                            //TODO: review here? 
                            //found this in xits.otf
                            _hintStemCount += (_current_integer_count / 2); //save a snapshot of stem count
                            _insts.AddOp(Operand.vstem);
                            _latestOpName = Operand.vstem;
                            _current_integer_count = 0;//clear
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {

                }
            }

            if (_hintStemCount == 0)
            {
                if (!_foundSomeStem)
                {
                    _hintStemCount = (_current_integer_count / 2);
                    if (_hintStemCount == 0)
                    {
                        return;
                    }
                    _foundSomeStem = true;//?
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            //---------------------- 
            //this is my hintmask extension, => to fit with our Evaluation stack
            int properNumberOfMaskBytes = (_hintStemCount + 7) / 8;

            if (reader.Position + properNumberOfMaskBytes >= reader.Length)
            {
                throw new NotSupportedException();
            }
            if (properNumberOfMaskBytes > 4)
            {
                int remaining = properNumberOfMaskBytes;

                for (; remaining > 3;)
                {
                    _insts.AddInt((
                       (reader.ReadByte() << 24) |
                       (reader.ReadByte() << 16) |
                       (reader.ReadByte() << 8) |
                       (reader.ReadByte())
                       ));
                    remaining -= 4; //*** 
                }
                switch (remaining)
                {
                    case 0:
                        //do nothing
                        break;
                    case 1:
                        _insts.AddInt(reader.ReadByte() << 24);
                        break;
                    case 2:
                        _insts.AddInt(
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16));

                        break;
                    case 3:
                        _insts.AddInt(
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16) |
                            (reader.ReadByte() << 8));
                        break;
                    default: throw new NotSupportedException();//should not occur !
                }

                _insts.AddOp(Operand.hintmask_bits, properNumberOfMaskBytes);
            }
            else
            {
                //last remaining <4 bytes 
                switch (properNumberOfMaskBytes)
                {
                    case 0:
                    default: throw new NotSupportedException();//should not occur !                     
                    case 1:
                        _insts.AddOp(Operand.hintmask1, (reader.ReadByte() << 24));
                        break;
                    case 2:
                        _insts.AddOp(Operand.hintmask2,
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16)
                            );
                        break;
                    case 3:
                        _insts.AddOp(Operand.hintmask3,
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16) |
                            (reader.ReadByte() << 8)
                            );
                        break;
                    case 4:
                        _insts.AddOp(Operand.hintmask4,
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16) |
                            (reader.ReadByte() << 8) |
                            (reader.ReadByte())
                            );
                        break;
                }
            }
        }
        /// <summary>
        /// cntrmask
        /// </summary>
        /// <param name="reader"></param>
        void AddCounterMaskToList(EndianReader reader)
        {
            if (_hintStemCount == 0)
            {
                if (!_foundSomeStem)
                {
                    //????
                    _hintStemCount = (_current_integer_count / 2);
                    _foundSomeStem = true;//?
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                _hintStemCount += (_current_integer_count / 2);
            }
            //---------------------- 
            //this is my hintmask extension, => to fit with our Evaluation stack
            int properNumberOfMaskBytes = (_hintStemCount + 7) / 8;
            if (reader.Position + properNumberOfMaskBytes >= reader.Length)
            {
                throw new NotSupportedException();
            }

            if (properNumberOfMaskBytes > 4)
            {
                int remaining = properNumberOfMaskBytes;

                for (; remaining > 3;)
                {
                    _insts.AddInt((
                       (reader.ReadByte() << 24) |
                       (reader.ReadByte() << 16) |
                       (reader.ReadByte() << 8) |
                       (reader.ReadByte())
                       ));
                    remaining -= 4; //*** 
                }
                switch (remaining)
                {
                    case 0:
                        //do nothing
                        break;
                    case 1:
                        _insts.AddInt(reader.ReadByte() << 24);
                        break;
                    case 2:
                        _insts.AddInt(
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16));

                        break;
                    case 3:
                        _insts.AddInt(
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16) |
                            (reader.ReadByte() << 8));
                        break;
                    default: throw new NotSupportedException();//should not occur !
                }

                _insts.AddOp(Operand.cntrmask_bits, properNumberOfMaskBytes);
            }
            else
            {
                //last remaining <4 bytes 
                switch (properNumberOfMaskBytes)
                {
                    case 0:
                    default: throw new NotSupportedException();//should not occur !
                    case 1:
                        _insts.AddOp(Operand.cntrmask1, (reader.ReadByte() << 24));
                        break;
                    case 2:
                        _insts.AddOp(Operand.cntrmask2,
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16)
                            );
                        break;
                    case 3:
                        _insts.AddOp(Operand.cntrmask3,
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16) |
                            (reader.ReadByte() << 8)
                            );
                        break;
                    case 4:
                        _insts.AddOp(Operand.cntrmask4,
                            (reader.ReadByte() << 24) |
                            (reader.ReadByte() << 16) |
                            (reader.ReadByte() << 8) |
                            (reader.ReadByte())
                            );
                        break;
                }
            }
        }

        static int ReadIntegerNumber(EndianReader reader, byte b0)
        {
            if (b0 >= 32 && b0 <= 246)
            {
                return b0 - 139;
            }
            else if (b0 <= 250)  // && b0 >= 247 , *** if-else sequence is important! ***
            {
                byte b1 = reader.ReadByte();
                return (b0 - 247) * 256 + b1 + 108;
            }
            else if (b0 <= 254)  //&&  b0 >= 251 ,*** if-else sequence is important! ***
            {
                byte b1 = reader.ReadByte();
                return -(b0 - 251) * 256 - b1 - 108;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
