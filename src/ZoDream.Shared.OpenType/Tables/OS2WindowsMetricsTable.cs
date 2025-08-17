using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class OS2WindowsMetricsTable : ITypefaceTable
    {
        public const string TableName = "OS/2";

        public string Name => TableName;

        public ushort Version {  get; set; }

        public short XAvgCharWidth;     //just average, not recommend to use.
        public ushort UsWeightClass;    //visual weight (degree of blackness or thickness of strokes), 0-1000

        //usWeightClass:
        //Value Description 	C Definition (from windows.h)
        //100 	Thin 	        FW_THIN
        //200 	Extra-light     FW_EXTRALIGHT
        //      (Ultra-light) 
        //300 	Light 	        FW_LIGHT
        //400 	Normal  	    FW_NORMAL
        //      (Regular)
        //500 	Medium 	        FW_MEDIUM
        //600 	Semi-bold   	FW_SEMIBOLD
        //      (Demi-bold)
        //700 	Bold 	        FW_BOLD
        //800 	Extra-bold  	FW_EXTRABOLD
        //      (Ultra-bold)
        //900 	Black (Heavy) 	FW_BLACK

        public ushort UsWidthClass;     //A relative change from the normal aspect ratio (width to height ratio), 
                                        //as specified by a font designer for the glyphs in a font.
                                        //Although every glyph in a font may have a different numeric aspect ratio, 
                                        //each glyph in a font of normal width is considered to have a relative aspect ratio of one.
                                        //When a new type style is created of a different width class (either by a font designer or by some automated means)
                                        //the relative aspect ratio of the characters in the new font is some percentage greater or less than those same characters in the normal 
                                        //font — it is this difference that this parameter specifies. 

        //usWidthClass
        //Value Description 	    C Definition 	        % of normal
        //1 	Ultra-condensed 	FWIDTH_ULTRA_CONDENSED 	50
        //2 	Extra-condensed 	FWIDTH_EXTRA_CONDENSED 	62.5
        //3 	Condensed 	        FWIDTH_CONDENSED 	    75
        //4 	Semi-condensed 	    FWIDTH_SEMI_CONDENSED 	87.5
        //5 	Medium (normal) 	FWIDTH_NORMAL 	        100
        //6 	Semi-expanded 	    FWIDTH_SEMI_EXPANDED 	112.5
        //7 	Expanded 	        FWIDTH_EXPANDED 	    125
        //8 	Extra-expanded 	    FWIDTH_EXTRA_EXPANDED 	150
        //9 	Ultra-expanded      FWIDTH_ULTRA_EXPANDED 	200


        public ushort FsType;           //Type flags., embedding licensing rights for the font


        public short YSubscriptXSize;
        public short YSubscriptYSize;
        public short YSubscriptXOffset;
        public short YSubscriptYOffset;
        public short YSuperscriptXSize;
        public short YSuperscriptYSize;
        public short YSuperscriptXOffset;
        public short YSuperscriptYOffset;
        public short YStrikeoutSize;
        public short YStrikeoutPosition;
        public short SFamilyClass;      //This parameter is a classification of font-family design. ,see https://www.microsoft.com/typography/otspec/ibmfc.htm

    
        public byte[] Panose;

        public uint UlUnicodeRange1;
        public uint UlUnicodeRange2;
        public uint UlUnicodeRange3;
        public uint UlUnicodeRange4;

        //Tag 	    achVendID[4] 	    char 4 
        public uint AchVendID;          //see 'registered venders' at https://www.microsoft.com/typography/links/vendorlist.aspx

        public ushort FsSelection;      //Contains information concerning the nature of the font patterns
        public ushort UsFirstCharIndex;
        public ushort UsLastCharIndex;
    
        public short STypoAscender;
        public short STypoDescender;
        public short STypoLineGap;
   
        public ushort UsWinAscent;
        public ushort UsWinDescent;
        public uint UlCodePageRange1;
        public uint UlCodePageRange2;
        
        public short SxHeight;
        public short SCapHeight;

        public ushort UsDefaultChar;
        public ushort UsBreakChar;
        public ushort UsMaxContext;
        public ushort UsLowerOpticalPointSize;
        public ushort UsUpperOpticalPointSize;
    }
}
