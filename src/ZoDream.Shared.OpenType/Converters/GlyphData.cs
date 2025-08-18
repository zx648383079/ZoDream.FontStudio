using System;
using System.Collections.Generic;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GlyphDataConverter : TypefaceTableConverter<GlyphDataTable>
    {
        public override GlyphDataTable? Read(EndianReader reader, ITypefaceTableEntry entry, ITypefaceTableSerializer serializer)
        {
            if (!serializer.TryGet<GlyphLocationsTable>(out var locations))
            {
                return null;
            }
            var res = new GlyphDataTable();

            uint tableOffset = (uint)reader.Position;
            int glyphCount = locations.Offsets.Length;
            res.Glyphs = new GlyphData[glyphCount];

            var compositeGlyphs = new List<ushort>();

            for (int i = 0; i < glyphCount; i++)
            {
                reader.BaseStream.Seek(tableOffset + locations.Offsets[i], SeekOrigin.Begin);//reset                  
                uint length = locations.Offsets[i + 1] - locations.Offsets[i];
                if (length > 0)
                {
                    //https://www.microsoft.com/typography/OTSPEC/glyf.htm
                    //header, 
                    //Type 	    Name 	            Description
                    //SHORT 	numberOfContours 	If the number of contours is greater than or equal to zero, this is a single glyph; if negative, this is a composite glyph.
                    //SHORT 	xMin 	            Minimum x for coordinate data.
                    //SHORT 	yMin 	            Minimum y for coordinate data.
                    //SHORT 	xMax 	            Maximum x for coordinate data.
                    //SHORT 	yMax 	            Maximum y for coordinate data.
                    short contoursCount = reader.ReadInt16();
                    if (contoursCount >= 0)
                    {
                        var bounds = reader.ReadBounds();
                        res.Glyphs[i] = ReadSimpleGlyph(reader, contoursCount, bounds, (ushort)i);
                    }
                    else
                    {
                        //skip composite glyph,
                        //resolve later
                        compositeGlyphs.Add((ushort)i);
                    }
                }
                else
                {
                    res.Glyphs[i] = GlyphData.Empty;
                }
            }

            //--------------------------------
            //resolve composte glyphs 
            //--------------------------------

            foreach (ushort glyphIndex in compositeGlyphs)
            {

#if DEBUG
                //if (glyphIndex == 7)
                //{ 
                //}
#endif
                res.Glyphs[glyphIndex] = ReadCompositeGlyph(res.Glyphs, locations, reader, tableOffset, glyphIndex);


            }

            return res;
        }

        private GlyphData ReadCompositeGlyph(GlyphData[] createdGlyphs, GlyphLocationsTable locations,   BinaryReader reader, uint tableOffset, ushort compositeGlyphIndex)
        {
            reader.BaseStream.Seek(tableOffset + locations.Offsets[compositeGlyphIndex], SeekOrigin.Begin);//reset
            //------------------------
            short contoursCount = reader.ReadInt16(); // ignored
            var bounds = reader.ReadBounds();

            GlyphData finalGlyph = null;
            CompositeGlyphFlags flags;

#if DEBUG
            int ncount = 0;
#endif
            do
            {
                flags = (CompositeGlyphFlags)reader.ReadUInt16();
                ushort glyphIndex = reader.ReadUInt16();
                if (createdGlyphs[glyphIndex] == null)
                {
                    // This glyph is not read yet, resolve it first!
                    long storedOffset = reader.BaseStream.Position;
                    var missingGlyph = ReadCompositeGlyph(createdGlyphs, locations, reader, tableOffset, glyphIndex);
                    createdGlyphs[glyphIndex] = missingGlyph;
                    reader.BaseStream.Position = storedOffset;
                }
                var newGlyph = TtfOutlineGlyphClone(createdGlyphs[glyphIndex], compositeGlyphIndex);

                int arg1 = 0;//arg1, arg2 may be int8,uint8,int16,uint 16 
                int arg2 = 0;//arg1, arg2 may be int8,uint8,int16,uint 16

                if (HasFlag(flags, CompositeGlyphFlags.ARG_1_AND_2_ARE_WORDS))
                {

                    //0x0002  ARGS_ARE_XY_VALUES Bit 1: If this is set,
                    //the arguments are **signed xy values**
                    //otherwise, they are unsigned point numbers.
                    if (HasFlag(flags, CompositeGlyphFlags.ARGS_ARE_XY_VALUES))
                    {
                        //singed
                        arg1 = reader.ReadInt16();
                        arg2 = reader.ReadInt16();
                    }
                    else
                    {
                        //unsigned
                        arg1 = reader.ReadUInt16();
                        arg2 = reader.ReadUInt16();
                    }
                }
                else
                {
                    //0x0002  ARGS_ARE_XY_VALUES Bit 1: If this is set,
                    //the arguments are **signed xy values**
                    //otherwise, they are unsigned point numbers.
                    if (HasFlag(flags, CompositeGlyphFlags.ARGS_ARE_XY_VALUES))
                    {
                        //singed
                        arg1 = (sbyte)reader.ReadByte();
                        arg2 = (sbyte)reader.ReadByte();
                    }
                    else
                    {
                        //unsigned
                        arg1 = reader.ReadByte();
                        arg2 = reader.ReadByte();
                    }
                }

                //-----------------------------------------
                float xscale = 1;
                float scale01 = 0;
                float scale10 = 0;
                float yscale = 1;

                bool useMatrix = false;
                //-----------------------------------------
                bool hasScale = false;
                if (HasFlag(flags, CompositeGlyphFlags.WE_HAVE_A_SCALE))
                {
                    //If the bit WE_HAVE_A_SCALE is set,
                    //the scale value is read in 2.14 format-the value can be between -2 to almost +2.
                    //The glyph will be scaled by this value before grid-fitting. 

                    xscale = yscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    hasScale = true;
                }
                else if (HasFlag(flags, CompositeGlyphFlags.WE_HAVE_AN_X_AND_Y_SCALE))
                {

                    xscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    yscale = reader.ReadF2Dot14(); /* Format 2.14 */
                    hasScale = true;
                }
                else if (HasFlag(flags, CompositeGlyphFlags.WE_HAVE_A_TWO_BY_TWO))
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
                    scale10 = reader.ReadF2Dot14();/* Format 2.14 */
                    yscale = reader.ReadF2Dot14(); /* Format 2.14 */

#if DEBUG
                    //TODO: review here
                    if (HasFlag(flags, CompositeGlyphFlags.UNSCALED_COMPONENT_OFFSET))
                    {


                    }
                    else
                    {


                    }
                    if (HasFlag(flags, CompositeGlyphFlags.USE_MY_METRICS))
                    {

                    }
#endif
                }

                //Argument1 and argument2 can be either...
                //   x and y offsets to be added to the glyph(the ARGS_ARE_XY_VALUES flag is set), 
                //or 
                //   two point numbers(the ARGS_ARE_XY_VALUES flag is **not** set)

                //When arguments 1 and 2 are an x and a y offset instead of points and the bit ROUND_XY_TO_GRID is set to 1,
                //the values are rounded to those of the closest grid lines before they are added to the glyph.
                //X and Y offsets are described in FUnits. 


                //--------------------------------------------------------------------
                if (HasFlag(flags, CompositeGlyphFlags.ARGS_ARE_XY_VALUES))
                {

                    if (useMatrix)
                    {
                        //use this matrix  
                        TtfTransformWith2x2Matrix(newGlyph, xscale, scale01, scale10, yscale);
                        TtfOffsetXY(newGlyph, (short)arg1, (short)arg2);
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
                            TtfOffsetXY(newGlyph, (short)arg1, (short)arg2);
                        }
                        else
                        {
                            if (HasFlag(flags, CompositeGlyphFlags.ROUND_XY_TO_GRID))
                            {
                                //TODO: implement round xy to grid***
                                //----------------------------
                            }
                            //just offset***
                            TtfOffsetXY(newGlyph, (short)arg1, (short)arg2);
                        }
                    }
                }
                else
                {
                    //two point numbers. 
                    //the first point number indicates the point that is to be matched to the new glyph. 
                    //The second number indicates the new glyph's “matched” point. 
                    //Once a glyph is added,its point numbers begin directly after the last glyphs (endpoint of first glyph + 1)

                    //TODO: implement this...

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


#if DEBUG
                ncount++;
#endif

            } while (HasFlag(flags, CompositeGlyphFlags.MORE_COMPONENTS));
            //
            if (HasFlag(flags, CompositeGlyphFlags.WE_HAVE_INSTRUCTIONS))
            {
                ushort numInstr = reader.ReadUInt16();
                byte[] insts = reader.ReadBytes(numInstr);
                finalGlyph.GlyphInstructions = insts;
            }
            //F2DOT14 	16-bit signed fixed number with the low 14 bits of fraction (2.14).
            //Transformation Option
            //
            //The C pseudo-code fragment below shows how the composite glyph information is stored and parsed; definitions for “flags” bits follow this fragment:
            //  do {
            //    USHORT flags;
            //    USHORT glyphIndex;
            //    if ( flags & ARG_1_AND_2_ARE_WORDS) {
            //    (SHORT or FWord) argument1;
            //    (SHORT or FWord) argument2;
            //    } else {
            //        USHORT arg1and2; /* (arg1 << 8) | arg2 */
            //    }
            //    if ( flags & WE_HAVE_A_SCALE ) {
            //        F2Dot14  scale;    /* Format 2.14 */
            //    } else if ( flags & WE_HAVE_AN_X_AND_Y_SCALE ) {
            //        F2Dot14  xscale;    /* Format 2.14 */
            //        F2Dot14  yscale;    /* Format 2.14 */
            //    } else if ( flags & WE_HAVE_A_TWO_BY_TWO ) {
            //        F2Dot14  xscale;    /* Format 2.14 */
            //        F2Dot14  scale01;   /* Format 2.14 */
            //        F2Dot14  scale10;   /* Format 2.14 */
            //        F2Dot14  yscale;    /* Format 2.14 */
            //    }
            //} while ( flags & MORE_COMPONENTS ) 
            //if (flags & WE_HAVE_INSTR){
            //    USHORT numInstr
            //    BYTE instr[numInstr]
            //------------------------------------------------------------ 


            return finalGlyph ?? GlyphData.Empty;
        }

        private static void TtfAppendGlyph(GlyphData dest, GlyphData src)
        {
            int org_dest_len = dest.EndPoints.Length;
#if DEBUG
            int src_contour_count = src.EndPoints.Length;
#endif
            if (org_dest_len == 0)
            {
                //org is empty glyph

                dest.GlyphPoints = [..dest.GlyphPoints, ..src.GlyphPoints];
                dest.EndPoints = [..dest.EndPoints, ..src.EndPoints];

            }
            else
            {
                ushort org_last_point = (ushort)(dest.EndPoints[org_dest_len - 1] + 1); //since start at 0 

                dest.GlyphPoints = [..dest.GlyphPoints, ..src.GlyphPoints];
                dest.EndPoints = [..dest.EndPoints, ..src.EndPoints];
                //offset latest append contour  end points
                int newlen = dest.EndPoints.Length;
                for (int i = org_dest_len; i < newlen; ++i)
                {
                    dest.EndPoints[i] += (ushort)org_last_point;
                }
            }



            //calculate new bounds
            var destBound = dest.Bounds;
            var srcBound = src.Bounds;
            short newXmin = (short)Math.Min(destBound.XMin, srcBound.XMin);
            short newYMin = (short)Math.Min(destBound.YMin, srcBound.YMin);
            short newXMax = (short)Math.Max(destBound.XMax, srcBound.XMax);
            short newYMax = (short)Math.Max(destBound.YMax, srcBound.YMax);

            dest.Bounds = new GlyphBound(newXmin, newYMin, newXMax, newYMax);
        }

        private static void TtfOffsetXY(GlyphData glyph, short dx, short dy)
        {
            //change data on current glyph
            var glyphPoints = glyph.GlyphPoints;
            for (int i = glyphPoints.Length - 1; i >= 0; --i)
            {
                glyphPoints[i] = glyphPoints[i].Offset(dx, dy);
            }
            //-------------------------
            var orgBounds = glyph.Bounds;
            glyph.Bounds = new GlyphBound(
               (short)(orgBounds.XMin + dx),
               (short)(orgBounds.YMin + dy),
               (short)(orgBounds.XMax + dx),
               (short)(orgBounds.YMax + dy));
        }

        private static void TtfTransformWith2x2Matrix(GlyphData glyph, float m00, float m01, float m10, float m11)
        {
            float new_xmin = 0;
            float new_ymin = 0;
            float new_xmax = 0;
            float new_ymax = 0;

            var glyphPoints = glyph.GlyphPoints;
            for (int i = 0; i < glyphPoints.Length; ++i)
            {
                var p = glyphPoints[i];
                float x = p.P.X;
                float y = p.P.Y;

                float newX, newY;
                //please note that this is transform normal***
                glyphPoints[i] = new GlyphPoint(
                   newX = (float)Math.Round((x * m00) + (y * m10)),
                   newY = (float)Math.Round((x * m01) + (y * m11)),
                   p.onCurve);

                //short newX = xs[i] = (short)Math.Round((x * m00) + (y * m10));
                //short newY = ys[i] = (short)Math.Round((x * m01) + (y * m11));
                //------
                if (newX < new_xmin)
                {
                    new_xmin = newX;
                }
                if (newX > new_xmax)
                {
                    new_xmax = newX;
                }
                //------
                if (newY < new_ymin)
                {
                    new_ymin = newY;
                }
                if (newY > new_ymax)
                {
                    new_ymax = newY;
                }
            }
            //TODO: review here
            glyph.Bounds = new GlyphBound(
               (short)new_xmin, (short)new_ymin,
               (short)new_xmax, (short)new_ymax);
        }

        private static GlyphData TtfOutlineGlyphClone(GlyphData original, ushort newGlyphIndex)
        {
            return new GlyphData(
              [..original.GlyphPoints],
              [..original.EndPoints],
              original.Bounds,
              original.GlyphInstructions != null ? [..original.GlyphInstructions] : null,
              newGlyphIndex);
        }

        private GlyphData ReadSimpleGlyph(EndianReader reader, int contourCount, GlyphBound bounds, ushort index)
        {
            ushort[] endPoints = reader.ReadArray(contourCount, reader.ReadUInt16);
            //-------------------------------------------------------
            ushort instructionLen = reader.ReadUInt16();
            byte[] instructions = reader.ReadBytes(instructionLen);
            //-------------------------------------------------------
            // TODO: should this take the max points rather?
            int pointCount = endPoints[contourCount - 1] + 1; // TODO: count can be zero?
            SimpleGlyphFlag[] flags = ReadFlags(reader, pointCount);
            short[] xs = ReadCoordinates(reader, pointCount, flags, SimpleGlyphFlag.XByte, SimpleGlyphFlag.XSignOrSame);
            short[] ys = ReadCoordinates(reader, pointCount, flags, SimpleGlyphFlag.YByte, SimpleGlyphFlag.YSignOrSame);

            int n = xs.Length;
            var glyphPoints = new GlyphPoint[n];
            for (int i = n - 1; i >= 0; --i)
            {
                glyphPoints[i] = new GlyphPoint(xs[i], ys[i], HasFlag(flags[i], SimpleGlyphFlag.OnCurve));
            }
            //-----------
            //lets build GlyphPoint set
            //-----------
            return new GlyphData(glyphPoints, endPoints, bounds, instructions, index);
        }

        private short[] ReadCoordinates(EndianReader reader, int pointCount, SimpleGlyphFlag[] flags, SimpleGlyphFlag isByte, SimpleGlyphFlag signOrSame)
        {
            var xs = new short[pointCount];
            int x = 0;
            for (int i = 0; i < pointCount; i++)
            {
                int dx;
                if (HasFlag(flags[i], isByte))
                {
                    byte b = reader.ReadByte();
                    dx = HasFlag(flags[i], signOrSame) ? b : -b;
                }
                else
                {
                    if (HasFlag(flags[i], signOrSame))
                    {
                        dx = 0;
                    }
                    else
                    {
                        dx = reader.ReadInt16();
                    }
                }
                x += dx;
                xs[i] = (short)x; // TODO: overflow?
            }
            return xs;
        }

        static bool HasFlag(SimpleGlyphFlag target, SimpleGlyphFlag test)
        {
            return (target & test) == test;
        }
        internal static bool HasFlag(CompositeGlyphFlags target, CompositeGlyphFlags test)
        {
            return (target & test) == test;
        }
        static SimpleGlyphFlag[] ReadFlags(BinaryReader input, int flagCount)
        {
            var result = new SimpleGlyphFlag[flagCount];
            int i = 0;
            int repeatCount = 0;
            var flag = (SimpleGlyphFlag)0;
            while (i < flagCount)
            {
                if (repeatCount > 0)
                {
                    repeatCount--;
                }
                else
                {
                    flag = (SimpleGlyphFlag)input.ReadByte();
                    if (HasFlag(flag, SimpleGlyphFlag.Repeat))
                    {
                        repeatCount = input.ReadByte();
                    }
                }
                result[i++] = flag;
            }
            return result;
        }

        public override void Write(EndianWriter writer, GlyphDataTable data)
        {
            throw new NotImplementedException();
        }
    }
}
