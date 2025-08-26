using System;
using System.Collections.Generic;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType;
using ZoDream.Shared.OpenType.Tables;
using BaseConverter = ZoDream.Shared.OpenType.Converters.GlyphDataConverter;

namespace ZoDream.Shared.WebType.Converters
{
    public class GlyphDataConverter : BaseConverter
    {
        const byte ONE_MORE_BYTE_CODE1 = 255;
        const byte ONE_MORE_BYTE_CODE2 = 254;
        const byte WORD_CODE = 253;
        const byte LOWEST_UCODE = 253;
        public override GlyphDataTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (entry is not WOFFTableEntry t || t.OriginalLength == 0)
            {
                return base.Read(reader, entry, serializer);
            }
            var res = new GlyphDataTable();
            long startAt = reader.BaseStream.Position;

            uint version = reader.ReadUInt32();
            ushort numGlyphs = reader.ReadUInt16();
            ushort indexFormatOffset = reader.ReadUInt16();

            uint nContourStreamSize = reader.ReadUInt32();
            uint nPointsStreamSize = reader.ReadUInt32();
            uint flagStreamSize = reader.ReadUInt32();
            uint glyphStreamSize = reader.ReadUInt32();
            uint compositeStreamSize = reader.ReadUInt32();
            uint bboxStreamSize = reader.ReadUInt32();
            uint instructionStreamSize = reader.ReadUInt32();

            long expected_nCountStartAt = reader.BaseStream.Position;
            long expected_nPointStartAt = expected_nCountStartAt + nContourStreamSize;
            long expected_FlagStreamStartAt = expected_nPointStartAt + nPointsStreamSize;
            long expected_GlyphStreamStartAt = expected_FlagStreamStartAt + flagStreamSize;
            long expected_CompositeStreamStartAt = expected_GlyphStreamStartAt + glyphStreamSize;

            long expected_BboxStreamStartAt = expected_CompositeStreamStartAt + compositeStreamSize;
            long expected_InstructionStreamStartAt = expected_BboxStreamStartAt + bboxStreamSize;
            long expected_EndAt = expected_InstructionStreamStartAt + instructionStreamSize;
            var glyphs = new GlyphData[numGlyphs];
            var allGlyphs = new TempGlyph[numGlyphs];
            var compositeGlyphs = new List<ushort>();
            int contourCount = 0;
            for (ushort i = 0; i < numGlyphs; ++i)
            {
                short numContour = reader.ReadInt16();
                allGlyphs[i] = new TempGlyph(i, numContour);
                if (numContour > 0)
                {
                    contourCount += numContour;
                    //>0 => simple glyph
                    //-1 = compound
                    //0 = empty glyph
                }
                else if (numContour < 0)
                {
                    //composite glyph, resolve later
                    compositeGlyphs.Add(i);
                }
                else
                {

                }
            }

            //1) nPoints stream,  npoint for each contour

            var pntPerContours = reader.ReadArray(contourCount, () => Read255UInt16(reader));
            //2) flagStream, flags value for each point
            //each byte in flags stream represents one point
            byte[] flagStream = reader.ReadBytes((int)flagStreamSize);

            using (var compositeMS = new MemoryStream())
            {
                reader.BaseStream.Position = expected_CompositeStreamStartAt;
                compositeMS.Write(reader.ReadBytes((int)compositeStreamSize), 0, (int)compositeStreamSize);
                compositeMS.Position = 0;

                int j = compositeGlyphs.Count;
                var compositeReader = new EndianReader(compositeMS, EndianType.BigEndian);
                for (ushort i = 0; i < j; ++i)
                {
                    ushort compositeGlyphIndex = compositeGlyphs[i];
                    allGlyphs[compositeGlyphIndex].CompositeHasInstructions = CompositeHasInstructions(compositeReader, compositeGlyphIndex);
                }
                reader.BaseStream.Position = expected_GlyphStreamStartAt;
            }
            //-------- 
            int curFlagsIndex = 0;
            int pntContourIndex = 0;
            for (int i = 0; i < allGlyphs.Length; ++i)
            {
                glyphs[i] = BuildSimpleGlyphStructure(reader,
                    ref allGlyphs[i],
                    GlyphData.Empty,
                    pntPerContours, ref pntContourIndex,
                    flagStream, ref curFlagsIndex);
            }
            {
                //now we read the composite stream again
                //and create composite glyphs
                int j = compositeGlyphs.Count;
                for (ushort i = 0; i < j; ++i)
                {
                    int compositeGlyphIndex = compositeGlyphs[i];
                    glyphs[compositeGlyphIndex] = ReadCompositeGlyph2(glyphs, reader, i, GlyphData.Empty);
                }
            }

            int bitmapCount = (numGlyphs + 7) / 8;
            byte[] bboxBitmap = ExpandBitmap(reader.ReadBytes(bitmapCount));
            for (ushort i = 0; i < numGlyphs; ++i)
            {
                TempGlyph tempGlyph = allGlyphs[i];
                var glyph = glyphs[i];

                byte hasBbox = bboxBitmap[i];
                if (hasBbox == 1)
                {
                    //read bbox from the bboxstream
                    glyph.Bounds = new GlyphBound(
                        reader.ReadInt16(),
                        reader.ReadInt16(),
                        reader.ReadInt16(),
                        reader.ReadInt16());
                }
                else
                {
                    //no bbox
                    //
                    if (tempGlyph.NumContour < 0)
                    {
                        //composite must have bbox
                        //if not=> err
                        throw new NotSupportedException();
                    }
                    else if (tempGlyph.NumContour > 0)
                    {
                        //simple glyph
                        //use simple calculation
                        //...For simple glyphs, if the corresponding bit in the bounding box bit vector is not set,
                        //then derive the bounding box by computing the minimum and maximum x and y coordinates in the outline, and storing that.
                        glyph.Bounds = FindSimpleGlyphBounds(glyph);
                    }
                }
            }

            reader.BaseStream.Position = expected_InstructionStreamStartAt;
            //--------------------------------------------------------------------------------------------

            for (ushort i = 0; i < numGlyphs; ++i)
            {
                TempGlyph tempGlyph = allGlyphs[i];
                if (tempGlyph.InstructionLen > 0)
                {
                    glyphs[i].GlyphInstructions = reader.ReadBytes(tempGlyph.InstructionLen);
                }
            }
            res.Glyphs = glyphs;
            return res;
        }

        private GlyphBound FindSimpleGlyphBounds(GlyphData glyph)
        {
            var glyphPoints = glyph.GlyphPoints;

            int j = glyphPoints.Length;
            float xmin = float.MaxValue;
            float ymin = float.MaxValue;
            float xmax = float.MinValue;
            float ymax = float.MinValue;

            for (int i = 0; i < j; ++i)
            {
                var p = glyphPoints[i];
                if (p.X < xmin) xmin = p.X;
                if (p.X > xmax) xmax = p.X;
                if (p.Y < ymin) ymin = p.Y;
                if (p.Y > ymax) ymax = p.Y;
            }

            return new GlyphBound(
               (short)Math.Round(xmin),
               (short)Math.Round(ymin),
               (short)Math.Round(xmax),
               (short)Math.Round(ymax));
        }

        private byte[] ExpandBitmap(byte[] orgBBoxBitmap)
        {
            var expandArr = new byte[orgBBoxBitmap.Length * 8];

            int index = 0;
            for (int i = 0; i < orgBBoxBitmap.Length; ++i)
            {
                byte b = orgBBoxBitmap[i];
                expandArr[index++] = (byte)((b >> 7) & 0x1);
                expandArr[index++] = (byte)((b >> 6) & 0x1);
                expandArr[index++] = (byte)((b >> 5) & 0x1);
                expandArr[index++] = (byte)((b >> 4) & 0x1);
                expandArr[index++] = (byte)((b >> 3) & 0x1);
                expandArr[index++] = (byte)((b >> 2) & 0x1);
                expandArr[index++] = (byte)((b >> 1) & 0x1);
                expandArr[index++] = (byte)((b >> 0) & 0x1);
            }
            return expandArr;
        }

        private GlyphData ReadCompositeGlyph2(GlyphData[] createdGlyphs, EndianReader reader, 
            ushort compositeGlyphIndex, GlyphData emptyGlyph)
        {
            GlyphData finalGlyph = null;
            CompositeGlyphFlags flags;
            do
            {
                flags = (CompositeGlyphFlags)reader.ReadUInt16();
                ushort glyphIndex = reader.ReadUInt16();
                if (createdGlyphs[glyphIndex] == null)
                {
                    // This glyph is not read yet, resolve it first!
                    long storedOffset = reader.BaseStream.Position;
                    var missingGlyph = ReadCompositeGlyph2(createdGlyphs, reader, glyphIndex, emptyGlyph);
                    createdGlyphs[glyphIndex] = missingGlyph;
                    reader.BaseStream.Position = storedOffset;
                }

                var newGlyph = TtfOutlineGlyphClone(createdGlyphs[glyphIndex], compositeGlyphIndex);

                short arg1 = 0;
                short arg2 = 0;
                ushort arg1and2 = 0;

                if (flags.HasFlag(CompositeGlyphFlags.ARG_1_AND_2_ARE_WORDS))
                {
                    arg1 = reader.ReadInt16();
                    arg2 = reader.ReadInt16();
                }
                else
                {
                    arg1and2 = reader.ReadUInt16();
                }
                //-----------------------------------------
                float xscale = 1;
                float scale01 = 0;
                float scale10 = 0;
                float yscale = 1;

                bool useMatrix = false;
                //-----------------------------------------
                bool hasScale = false;
                if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_A_SCALE))
                {
                    //If the bit WE_HAVE_A_SCALE is set,
                    //the scale value is read in 2.14 format-the value can be between -2 to almost +2.
                    //The glyph will be scaled by this value before grid-fitting. 
                    xscale = yscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    hasScale = true;
                }
                else if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_AN_X_AND_Y_SCALE))
                {
                    xscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    yscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    hasScale = true;
                }
                else if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_A_TWO_BY_TWO))
                {

                    //The bit WE_HAVE_A_TWO_BY_TWO allows for linear transformation of the X and Y coordinates by specifying a 2 × 2 matrix.
                    //This could be used for scaling and 90-degree*** rotations of the glyph components, for example.

                    //2x2 matrix

                    //The purpose of USE_MY_METRICS is to force the lsb and rsb to take on a desired value.
                    //For example, an i-circumflex (U+00EF) is often composed of the circumflex and a dotless-i. 
                    //In order to force the composite to have the same metrics as the dotless-i,
                    //set USE_MY_METRICS for the dotless-i component of the composite. 
                    //Without this bit, the rsb and lsb would be calculated from the hmtx entry for the composite 
                    //(or would need to be explicitly set with TrueType instructions).

                    //Note that the behavior of the USE_MY_METRICS operation is undefined for rotated composite components. 
                    useMatrix = true;
                    hasScale = true;
                    xscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    scale01 = reader.ReadF2Dot14(); /* Format 2.14 */
                    scale10 = reader.ReadF2Dot14(); /* Format 2.14 */
                    yscale = reader.ReadF2Dot14(); /* Format 2.14 */

                    if (flags.HasFlag(CompositeGlyphFlags.UNSCALED_COMPONENT_OFFSET))
                    {


                    }
                    else
                    {


                    }
                    if (flags.HasFlag(CompositeGlyphFlags.USE_MY_METRICS))
                    {

                    }
                }

                //--------------------------------------------------------------------
                if (flags.HasFlag(CompositeGlyphFlags.ARGS_ARE_XY_VALUES))
                {
                    //Argument1 and argument2 can be either x and y offsets to be added to the glyph or two point numbers.  
                    //x and y offsets to be added to the glyph
                    //When arguments 1 and 2 are an x and a y offset instead of points and the bit ROUND_XY_TO_GRID is set to 1,
                    //the values are rounded to those of the closest grid lines before they are added to the glyph.
                    //X and Y offsets are described in FUnits. 

                    if (useMatrix)
                    {
                        //use this matrix  
                        TtfTransformWith2x2Matrix(newGlyph, xscale, scale01, scale10, yscale);
                        TtfOffsetXY(newGlyph, arg1, arg2);
                    }
                    else
                    {
                        if (hasScale)
                        {
                            if (xscale == 1.0 && yscale == 1.0)
                            {

                            }
                            else
                            {
                                TtfTransformWith2x2Matrix(newGlyph, xscale, 0, 0, yscale);
                            }
                            TtfOffsetXY(newGlyph, arg1, arg2);
                        }
                        else
                        {
                            if (flags.HasFlag(CompositeGlyphFlags.ROUND_XY_TO_GRID))
                            {
                                //TODO: implement round xy to grid***
                                //----------------------------
                            }
                            //just offset***
                            TtfOffsetXY(newGlyph, arg1, arg2);
                        }
                    }


                }
                else
                {
                    //two point numbers. 
                    //the first point number indicates the point that is to be matched to the new glyph. 
                    //The second number indicates the new glyph's “matched” point. 
                    //Once a glyph is added,its point numbers begin directly after the last glyphs (endpoint of first glyph + 1)

                }

                //
                if (finalGlyph == null)
                {
                    finalGlyph = newGlyph;
                }
                else
                {
                    //merge 
                    TtfAppendGlyph(finalGlyph, newGlyph);
                }

            } while (flags.HasFlag(CompositeGlyphFlags.MORE_COMPONENTS));

            //
            if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_INSTRUCTIONS))
            {
                //read this later
                //ushort numInstr = reader.ReadUInt16();
                //byte[] insts = reader.ReadBytes(numInstr);
                //finalGlyph.GlyphInstructions = insts;
            }


            return finalGlyph ?? emptyGlyph;
        }

        private GlyphData BuildSimpleGlyphStructure(EndianReader reader,
            ref TempGlyph tmpGlyph,
            GlyphData emptyGlyph,
            ushort[] pntPerContours, ref int pntContourIndex,
            byte[] flagStream, ref int flagStreamIndex)
        {
            if (tmpGlyph.NumContour == 0) return emptyGlyph;
            if (tmpGlyph.NumContour < 0)
            {
                //composite glyph,
                //check if this has instruction or not
                if (tmpGlyph.CompositeHasInstructions)
                {
                    tmpGlyph.InstructionLen = Read255UInt16(reader);
                }
                return null;//skip composite glyph (resolve later)     
            }

            //-----
            int curX = 0;
            int curY = 0;

            int numContour = tmpGlyph.NumContour;

            var _endContours = new ushort[numContour];
            ushort pointCount = 0;

            //create contours
            for (ushort i = 0; i < numContour; ++i)
            {
                ushort numPoint = pntPerContours[pntContourIndex++];//increament pntContourIndex AFTER
                pointCount += numPoint;
                _endContours[i] = (ushort)(pointCount - 1);
            }

            //collect point for our contours
            var _glyphPoints = new GlyphPoint[pointCount];
            int n = 0;
            for (int i = 0; i < numContour; ++i)
            {
                //read point detail
                //step 3) 

                //foreach contour
                //read 1 byte flags for each contour

                //1) The most significant bit of a flag indicates whether the point is on- or off-curve point,
                //2) the remaining seven bits of the flag determine the format of X and Y coordinate values and 
                //specify 128 possible combinations of indices that have been assigned taking into consideration 
                //typical statistical distribution of data found in TrueType fonts. 

                //When X and Y coordinate values are recorded using nibbles(either 4 bits per coordinate or 12 bits per coordinate)
                //the bits are packed in the byte stream with most significant bit of X coordinate first, 
                //followed by the value for Y coordinate (most significant bit first). 
                //As a result, the size of the glyph dataset is significantly reduced, 
                //and the grouping of the similar values(flags, coordinates) in separate and contiguous data streams allows 
                //more efficient application of the entropy coding applied as the second stage of encoding process. 

                int endContour = _endContours[i];
                for (; n <= endContour; ++n)
                {

                    byte f = flagStream[flagStreamIndex++]; //increment the flagStreamIndex AFTER read

                    //int f1 = (f >> 7); // most significant 1 bit -> on/off curve

                    int xyFormat = f & 0x7F; // remainging 7 bits x,y format  

                    var enc = TripleEncodingTable.Instance.Value[xyFormat]; //0-128 

                    var packedXY = reader.ReadBytes(enc.ByteCount - 1); //byte count include 1 byte flags, so actual read=> byteCount-1
                                                                                      //read x and y 

                    int x = 0;
                    int y = 0;

                    switch (enc.XBits)
                    {
                        default:
                            throw new NotSupportedException();//???
                        case 0: //0,8, 
                            x = 0;
                            y = enc.Ty(packedXY[0]);
                            break;
                        case 4: //4,4
                            x = enc.Tx(packedXY[0] >> 4);
                            y = enc.Ty(packedXY[0] & 0xF);
                            break;
                        case 8: //8,0 or 8,8
                            x = enc.Tx(packedXY[0]);
                            y = (enc.YBits == 8) ?
                                    enc.Ty(packedXY[1]) :
                                    0;
                            break;
                        case 12: //12,12
                                 //x = enc.Tx((packedXY[0] << 8) | (packedXY[1] >> 4));
                                 //y = enc.Ty(((packedXY[1] & 0xF)) | (packedXY[2] >> 4));
                            x = enc.Tx((packedXY[0] << 4) | (packedXY[1] >> 4));
                            y = enc.Ty(((packedXY[1] & 0xF) << 8) | (packedXY[2]));
                            break;
                        case 16: //16,16
                            x = enc.Tx((packedXY[0] << 8) | packedXY[1]);
                            y = enc.Ty((packedXY[2] << 8) | packedXY[3]);
                            break;
                    }

                    //incremental point format***
                    _glyphPoints[n] = new GlyphPoint(curX += x, curY += y, (f >> 7) == 0); // most significant 1 bit -> on/off curve 
                }
            }

            //----
            //step 4) Read one 255UInt16 value from the glyph stream, which is instructionLength, the number of instruction bytes.
            tmpGlyph.InstructionLen = Read255UInt16(reader);
            //step 5) resolve it later

            return new GlyphData(_glyphPoints,
               _endContours,
               new GlyphBound(), //calculate later
               null,  //load instruction later
               tmpGlyph.GlyphIndex);
        }

        private bool CompositeHasInstructions(EndianReader reader, ushort compositeGlyphIndex)
        {
            CompositeGlyphFlags flags;
            do
            {
                flags = (CompositeGlyphFlags)reader.ReadUInt16();
                ushort glyphIndex = reader.ReadUInt16();
                short arg1 = 0;
                short arg2 = 0;
                ushort arg1and2 = 0;

                if (flags.HasFlag(CompositeGlyphFlags.ARG_1_AND_2_ARE_WORDS))
                {
                    arg1 = reader.ReadInt16();
                    arg2 = reader.ReadInt16();
                }
                else
                {
                    arg1and2 = reader.ReadUInt16();
                }
                //-----------------------------------------
                float xscale = 1;
                float scale01 = 0;
                float scale10 = 0;
                float yscale = 1;

                bool useMatrix = false;
                //-----------------------------------------
                bool hasScale = false;
                if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_A_SCALE))
                {
                    //If the bit WE_HAVE_A_SCALE is set,
                    //the scale value is read in 2.14 format-the value can be between -2 to almost +2.
                    //The glyph will be scaled by this value before grid-fitting. 
                    xscale = yscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    hasScale = true;
                }
                else if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_AN_X_AND_Y_SCALE))
                {
                    xscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    yscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    hasScale = true;
                }
                else if (flags.HasFlag(CompositeGlyphFlags.WE_HAVE_A_TWO_BY_TWO))
                {
                    useMatrix = true;
                    hasScale = true;
                    xscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    scale01 = reader.ReadF2Dot14(); /* Format 2.14 */
                    scale10 = reader.ReadF2Dot14();/* Format 2.14 */
                    yscale = reader.ReadF2Dot14(); /* Format 2.14 */

                }

            } while (flags.HasFlag(CompositeGlyphFlags.MORE_COMPONENTS));

            return flags.HasFlag(CompositeGlyphFlags.WE_HAVE_INSTRUCTIONS);
        }

        private static ushort Read255UInt16(EndianReader reader)
        {
            var code = reader.ReadByte();
            if (code == WORD_CODE)
            {
                int value = reader.ReadByte();
                value <<= 8;
                value &= 0xff00;
                int value2 = reader.ReadByte();
                value |= value2 & 0x00ff;

                return (ushort)value;
            }
            else if (code == ONE_MORE_BYTE_CODE1)
            {
                return (ushort)(reader.ReadByte() + LOWEST_UCODE);
            }
            else if (code == ONE_MORE_BYTE_CODE2)
            {
                return (ushort)(reader.ReadByte() + (LOWEST_UCODE * 2));
            }
            else
            {
                return code;
            }
        }

        struct TempGlyph
        {
            public readonly ushort GlyphIndex;
            public readonly short NumContour;

            public ushort InstructionLen;
            public bool CompositeHasInstructions;
            public TempGlyph(ushort glyphIndex, short contourCount)
            {
                GlyphIndex = glyphIndex;
                NumContour = contourCount;

                InstructionLen = 0;
                CompositeHasInstructions = false;
            }
        }
    }
}
