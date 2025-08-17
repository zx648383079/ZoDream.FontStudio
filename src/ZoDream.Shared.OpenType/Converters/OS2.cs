using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class OS2Converter : TypefaceConverter<OS2WindowsMetricsTable>
    {
        public override OS2WindowsMetricsTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new OS2WindowsMetricsTable();
            res.Version = reader.ReadUInt16();

            res.XAvgCharWidth = reader.ReadInt16();
            res.UsWeightClass = reader.ReadUInt16();
            res.UsWidthClass = reader.ReadUInt16();
            res.FsType = reader.ReadUInt16();

            res.YSubscriptXSize = reader.ReadInt16();
            res.YSubscriptYSize = reader.ReadInt16();
            res.YSubscriptXOffset = reader.ReadInt16();
            res.YSubscriptYOffset = reader.ReadInt16();
            res.YSuperscriptXSize = reader.ReadInt16();
            res.YSuperscriptYSize = reader.ReadInt16();
            res.YSuperscriptXOffset = reader.ReadInt16();
            res.YSuperscriptYOffset = reader.ReadInt16();
            res.YStrikeoutSize = reader.ReadInt16();
            res.YStrikeoutPosition = reader.ReadInt16();
            res.SFamilyClass = reader.ReadInt16();
	 
            res.Panose = reader.ReadBytes(10);

            res.UlUnicodeRange1 = reader.ReadUInt32();
            if (res.Version >= 1)
            {
                res.UlUnicodeRange2 = reader.ReadUInt32();
                res.UlUnicodeRange3 = reader.ReadUInt32();
                res.UlUnicodeRange4 = reader.ReadUInt32();
            }

            //CHAR 	achVendID[4] 	 
            res.AchVendID = reader.ReadUInt32();
            //USHORT 	fsSelection 	 
            //USHORT 	usFirstCharIndex 	 
            //USHORT 	usLastCharIndex 	 
            res.FsSelection = reader.ReadUInt16();
            res.UsFirstCharIndex = reader.ReadUInt16();
            res.UsLastCharIndex = reader.ReadUInt16();
            //SHORT 	sTypoAscender 	 
            //SHORT 	sTypoDescender 	 
            //SHORT 	sTypoLineGap 	 
            res.STypoAscender = reader.ReadInt16();
            res.STypoDescender = reader.ReadInt16();
            res.STypoLineGap = reader.ReadInt16();
            //USHORT 	usWinAscent 	 
            //USHORT 	usWinDescent
            res.UsWinAscent = reader.ReadUInt16();
            res.UsWinDescent = reader.ReadUInt16();

            if (res.Version >= 1)
            {
                res.UlCodePageRange1 = reader.ReadUInt32();
                res.UlCodePageRange2 = reader.ReadUInt32();
            }

            if (res.Version >= 2)
            {
                res.SxHeight = reader.ReadInt16();
                res.SCapHeight = reader.ReadInt16();
                res.UsDefaultChar = reader.ReadUInt16();
                res.UsBreakChar = reader.ReadUInt16();
                res.UsMaxContext = reader.ReadUInt16();
            }
            if (res.Version >= 5)
            {
                res.UsLowerOpticalPointSize = reader.ReadUInt16();
                res.UsUpperOpticalPointSize = reader.ReadUInt16();
            }
            return res;
        }

        public override void Write(EndianWriter writer, OS2WindowsMetricsTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
