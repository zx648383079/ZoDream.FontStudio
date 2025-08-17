using System.Collections.Generic;
using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class NameTable : ITypefaceTable
    {
        public const string TableName = "name";

        public string Name => TableName;

        public Dictionary<NameIdKind, string> Items { get; set; } = [];
    }

    internal struct NameRecord
    {
        public ushort PlatformID;
        public ushort EncodingID;
        public ushort LanguageID;
        public ushort NameID;
        public ushort StringLength;
        public ushort StringOffset;
    }

    public enum NameIdKind
    {
        //...
        //[A] The key information for this table for Microsoft platforms 
        //relates to the use of strings 1, 2, 4, 16 and 17.
        //...


        CopyRightNotice, //0
        FontFamilyName, //1 , [A]
        FontSubfamilyName,//2, [A]
        UniqueFontIden, //3
        FullFontName, //4, [A]
        VersionString,//5
        PostScriptName,//6
        Trademark,//7
        ManufacturerName,//8
        Designer,//9
        Description, //10
        UrlVendor, //11
        UrlDesigner,//12
        LicenseDescription, //13
        LicenseInfoUrl,//14
        Reserved,//15
        TypographicFamilyName,//16 , [A]
        TypographyicSubfamilyName,//17, [A]
        CompatibleFull,//18
        SampleText,//19
        PostScriptCID_FindfontName,//20
                                   //------------------            
        WWSFamilyName,//21
        WWSSubfamilyName,//22
                         //------------------
        LightBackgroundPalette,//23, CPAL
        DarkBackgroundPalette,//24, CPAL
                              //------------------
        VariationsPostScriptNamePrefix,//25

    }
}
