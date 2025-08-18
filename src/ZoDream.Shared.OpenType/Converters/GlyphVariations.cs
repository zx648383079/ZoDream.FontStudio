using System;
using System.Collections.Generic;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class GlyphVariationsConverter : TypefaceConverter<GlyphVariationsTable>
    {
        public override GlyphVariationsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new GlyphVariationsTable();
            long beginAt = reader.BaseStream.Position;

            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
#if DEBUG
            if (majorVersion != 1 && minorVersion != 0)
            {
                //WARN
            }
#endif

            res.axisCount = reader.ReadUInt16(); //This must be the same number as axisCount in the 'fvar' table

            ushort sharedTupleCount = reader.ReadUInt16();
            uint sharedTuplesOffset = reader.ReadUInt32();
            ushort glyphCount = reader.ReadUInt16();
            ushort flags = reader.ReadUInt16();
            uint glyphVariationDataArrayOffset = reader.ReadUInt32();

            uint[] glyphVariationDataOffsets = null;
            if ((flags & 0x1) == 0)
            {
                //bit 0 is clear-> use Offset16
                glyphVariationDataOffsets = reader.ReadArray(glyphCount, () => (uint)reader.ReadUInt16());
                //
                //***If the short format (Offset16) is used for offsets, 
                //the value stored is the offset divided by 2.
                //Hence, the actual offset for the location of the GlyphVariationData table within the font 
                //will be the value stored in the offsets array multiplied by 2.

                for (int i = 0; i < glyphVariationDataOffsets.Length; ++i)
                {
                    glyphVariationDataOffsets[i] *= 2;
                }
            }
            else
            {
                //Offset32
                glyphVariationDataOffsets = reader.ReadArray(glyphCount, reader.ReadUInt32);
            }

            reader.BaseStream.Position = beginAt + sharedTuplesOffset;
            var tupleRecords = new TupleRecord[sharedTupleCount];
            for (int t = 0; t < sharedTupleCount; ++t)
            {
                tupleRecords[t] = new TupleRecord(reader.ReadArray(res.axisCount, reader.ReadF2Dot14));
            }

            res._sharedTuples = tupleRecords;


            //GlyphVariationData array ... 
            long glyphVariableData_startAt = beginAt + glyphVariationDataArrayOffset;
            reader.BaseStream.Position = glyphVariableData_startAt;

            res._glyphVarDataArr = new GlyphVariableData[glyphVariationDataOffsets.Length];

            for (int i = 0; i < glyphVariationDataOffsets.Length; ++i)
            {
                reader.BaseStream.Position = glyphVariableData_startAt + glyphVariationDataOffsets[i];
                res._glyphVarDataArr[i] = ReadGlyphVariationData(reader, res.axisCount);
            }
            return res;
        }

        private GlyphVariableData ReadGlyphVariationData(EndianReader reader, ushort axisCount)
        {
            var glyphVarData = new GlyphVariableData();

            long beginAt = reader.BaseStream.Position;
            ushort tupleVariationCount = reader.ReadUInt16();
            ushort dataOffset = reader.ReadUInt16();

            int tupleCount = tupleVariationCount & 0xFFF;//low 12 bits are the number of tuple variation tables for this glyph

            TupleVariationHeader[] tupleHaders = new TupleVariationHeader[tupleCount];
            glyphVarData.tupleHeaders = tupleHaders;

            for (int i = 0; i < tupleCount; ++i)
            {
                tupleHaders[i] = ReadTupleVariationHeader(reader, axisCount);
            }
            reader.BaseStream.Position = beginAt + dataOffset;
            int flags = tupleVariationCount >> 12;//The high 4 bits are flags, 
            if ((flags & 0x8) == 0x8)//check the flags has SHARED_POINT_NUMBERS or not
            {

                //The serialized data block begins with shared “point” number data, 
                //followed by the variation data for the tuple variation tables.
                //The shared point number data is optional:
                //it is present if the corresponding flag is set in the tupleVariationCount field of the header.
                //If present, the shared number data is represented as packed point numbers, described below.

                //https://docs.microsoft.com/en-gb/typography/opentype/spec/otvarcommonformats#packed-point-numbers

                //...
                //Packed point numbers are stored as a count followed by one or more runs of point number data.

                //The count may be stored in one or two bytes.
                //After reading the first byte, the need for a second byte can be determined. 
                //The count bytes are processed as follows: 
                glyphVarData._sharedPoints = ReadPackedPoints(reader);
            }

            for (int i = 0; i < tupleCount; ++i)
            {
                TupleVariationHeader header = tupleHaders[i];

                ushort dataSize = header.variableDataSize;
                long expect_endAt = reader.BaseStream.Position + dataSize;

#if DEBUG
                if (expect_endAt > reader.BaseStream.Length)
                {

                }
#endif
                //The variationDataSize value indicates the size of serialized data for the given tuple variation table that is contained in the serialized data. 
                //It does not include the size of the TupleVariationHeader.

                if ((header.flags & ((int)TupleIndexFormat.PRIVATE_POINT_NUMBERS >> 12)) == ((int)TupleIndexFormat.PRIVATE_POINT_NUMBERS) >> 12)
                {
                    header.PrivatePoints = ReadPackedPoints(reader);
                }
                else if (header.flags != 0)
                {

                }

                var packedDeltasXY = new List<short>();
                while (reader.BaseStream.Position < expect_endAt)
                {
                    byte controlByte = reader.ReadByte();
                    int number_in_run = (controlByte & 0x3F) + 1;

                    int flags01 = (controlByte >> 6) << 6;

                    if (flags01 == 0x80)
                    {
                        for (int nn = 0; nn < number_in_run; ++nn)
                        {
                            packedDeltasXY.Add(0);
                        }
                    }
                    else if (flags01 == 0x40)
                    {
                        //DELTAS_ARE_WORDS Flag indicating the data type for delta values in the run.If set,
                        //the run contains 16 - bit signed deltas(int16);
                        //if clear, the run contains 8 - bit signed deltas(int8).

                        for (int nn = 0; nn < number_in_run; ++nn)
                        {
                            packedDeltasXY.Add(reader.ReadInt16());
                        }
                    }
                    else if (flags01 == 0)
                    {
                        for (int nn = 0; nn < number_in_run; ++nn)
                        {
                            packedDeltasXY.Add(reader.ReadByte());
                        }
                    }
                    else
                    {

                    }
                }
                //---
                header.PackedDeltasXY = packedDeltasXY.ToArray();

#if DEBUG
                //ensure!
                if ((packedDeltasXY.Count % 2) != 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
                //ensure!
                if (reader.BaseStream.Position != expect_endAt)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
            }

            return glyphVarData;
        }

        private TupleVariationHeader ReadTupleVariationHeader(EndianReader reader, ushort axisCount)
        {
            var header = new TupleVariationHeader();

            header.variableDataSize = reader.ReadUInt16();
            ushort tupleIndex = reader.ReadUInt16();
            int flags = (tupleIndex >> 12) & 0xF; //The high 4 bits are flags(see below).
            header.flags = flags; //The high 4 bits are flags(see below).
            header.indexToSharedTupleRecArray = (ushort)(tupleIndex & 0x0FFF); // The low 12 bits are an index into a shared tuple records array.


            if ((flags & ((int)TupleIndexFormat.EMBEDDED_PEAK_TUPLE >> 12)) == ((int)TupleIndexFormat.EMBEDDED_PEAK_TUPLE >> 12))
            {
                //TODO:...
                header.peakTuple = new TupleRecord(reader.ReadArray(axisCount, reader.ReadF2Dot14));
            }
            if ((flags & ((int)TupleIndexFormat.INTERMEDIATE_REGION >> 12)) == ((int)TupleIndexFormat.INTERMEDIATE_REGION >> 12))
            {
                //TODO:...
                header.intermediateStartTuple = new TupleRecord(reader.ReadArray(axisCount, reader.ReadF2Dot14));
                header.intermediateEndTuple = new TupleRecord(reader.ReadArray(axisCount, reader.ReadF2Dot14));
            }

            return header;
        }

        private ushort[] ReadPackedPoints(EndianReader reader)
        {
            byte b0 = reader.ReadByte();
            if (b0 == 0)
            {
                return [];
            }
            else if (b0 > 0 && b0 <= 127)
            {
                return ReadPackedPoints(reader, b0);
            }
            else
            {
                //If the high bit of the first byte is set, then a second byte is used.
                //The count is read from interpreting the two bytes as a big-endian uint16 value with the high-order bit masked out.  

                //Thus, if the count fits in 7 bits, it is stored in a single byte, with the value 0 having a special interpretation.
                //If the count does not fit in 7 bits, then the count is stored in the first two bytes with the high bit of the first byte set as a flag 
                //that is not part of the count — the count uses 15 bits.

                byte b1 = reader.ReadByte();
                return ReadPackedPoints(reader, ((b0 & 0x7F) << 8) | b1);
            }
        }

        private ushort[] ReadPackedPoints(EndianReader reader, int point_count)
        {
            var res = new List<ushort>();
            int point_read = 0;
            //for (int n = 0; n < point_count ; ++n)
            while (point_read < point_count)
            {
                //Point number data runs follow after the count.

                //Each data run begins with a control byte that specifies the number of point numbers defined in the run,
                //and a flag bit indicating the format of the run data. 
                //The control byte’s high bit specifies whether the run is represented in 8-bit or 16-bit values. 
                //The low 7 bits specify the number of elements in the run minus 1.
                //The format of the control byte is as follows:

                byte controlByte = reader.ReadByte();

                //Mask 	Name 	                Description
                //0x80 	POINTS_ARE_WORDS 	    Flag indicating the data type used for point numbers in this run.
                //                              If set, the point numbers are stored as unsigned 16-bit values (uint16); 
                //                              if clear, the point numbers are stored as unsigned bytes (uint8).
                //0x7F 	POINT_RUN_COUNT_MASK 	Mask for the low 7 bits of the control byte to give the number of point number elements, minus 1.


                int point_run_count = (controlByte & 0x7F) + 1;
                //In the first point run, the first point number is represented directly (that is, as a difference from zero). 
                //Each subsequent point number in that run is stored as the difference between it and the previous point number. 
                //In subsequent runs, all elements, including the first, represent a difference from the last point number.

                if (((controlByte & 0x80) == 0x80)) //point_are_uint16
                {
                    for (int i = 0; i < point_run_count; ++i)
                    {
                        point_read++;
                        res.Add(reader.ReadUInt16());
                    }
                }
                else
                {
                    for (int i = 0; i < point_run_count; ++i)
                    {
                        point_read++;
                        res.Add(reader.ReadByte());
                    }
                }
            }
            return [.. res];
        }

        public override void Write(EndianWriter writer, GlyphVariationsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
