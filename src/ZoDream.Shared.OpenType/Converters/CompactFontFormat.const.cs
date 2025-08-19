using System.Collections.Generic;
using ZoDream.Shared.OpenType.CompactFontFormat;

namespace ZoDream.Shared.OpenType.Converters
{
    public partial class CompactFontFormatConverter
    {
        static CompactFontFormatConverter()
        {
            //Table 9: Top DICT Operator Entries          
            Register(0, "version", OperatorOperandKind.SID);
            Register(1, "Notice", OperatorOperandKind.SID);
            Register(12, 0, "Copyright", OperatorOperandKind.SID);
            Register(2, "FullName", OperatorOperandKind.SID);
            Register(3, "FamilyName", OperatorOperandKind.SID);
            Register(4, "Weight", OperatorOperandKind.SID);
            Register(12, 1, "isFixedPitch", OperatorOperandKind.Boolean);
            Register(12, 2, "ItalicAngle", OperatorOperandKind.Number);
            Register(12, 3, "UnderlinePosition", OperatorOperandKind.Number);
            Register(12, 4, "UnderlineThickness", OperatorOperandKind.Number);
            Register(12, 5, "PaintType", OperatorOperandKind.Number);
            Register(12, 6, "CharstringType", OperatorOperandKind.Number); //default value 2
            Register(12, 7, "FontMatrix", OperatorOperandKind.Array);
            Register(13, "UniqueID", OperatorOperandKind.Number);
            Register(5, "FontBBox", OperatorOperandKind.Array);
            Register(12, 8, "StrokeWidth", OperatorOperandKind.Number);
            Register(14, "XUID", OperatorOperandKind.Array);
            Register(15, "charset", OperatorOperandKind.Number);
            Register(16, "Encoding", OperatorOperandKind.Number);
            Register(17, "CharStrings", OperatorOperandKind.Number);
            Register(18, "Private", OperatorOperandKind.NumberNumber);
            Register(12, 20, "SyntheticBase", OperatorOperandKind.Number);
            Register(12, 21, "PostScript", OperatorOperandKind.SID);
            Register(12, 22, "BaseFontName", OperatorOperandKind.SID);
            Register(12, 23, "BaseFontBlend", OperatorOperandKind.SID);

            //Table 10: CIDFont Operator Extensions
            Register(12, 30, "ROS", OperatorOperandKind.SID_SID_Number);
            Register(12, 31, "CIDFontVersion", OperatorOperandKind.Number);
            Register(12, 32, "CIDFontRevision", OperatorOperandKind.Number);
            Register(12, 33, "CIDFontType", OperatorOperandKind.Number);
            Register(12, 34, "CIDCount", OperatorOperandKind.Number);
            Register(12, 35, "UIDBase", OperatorOperandKind.Number);
            Register(12, 36, "FDArray", OperatorOperandKind.Number);
            Register(12, 37, "FDSelect", OperatorOperandKind.Number);
            Register(12, 38, "FontName", OperatorOperandKind.SID);

            //Table 23: Private DICT Operators
            Register(6, "BlueValues", OperatorOperandKind.Delta);
            Register(7, "OtherBlues", OperatorOperandKind.Delta);
            Register(8, "FamilyBlues", OperatorOperandKind.Delta);
            Register(9, "FamilyOtherBlues", OperatorOperandKind.Delta);
            Register(12, 9, "BlueScale", OperatorOperandKind.Number);
            Register(12, 10, "BlueShift", OperatorOperandKind.Number);
            Register(12, 11, "BlueFuzz", OperatorOperandKind.Number);
            Register(10, "StdHW", OperatorOperandKind.Number);
            Register(11, "StdVW", OperatorOperandKind.Number);
            Register(12, 12, "StemSnapH", OperatorOperandKind.Delta);
            Register(12, 13, "StemSnapV", OperatorOperandKind.Delta);
            Register(12, 14, "ForceBold", OperatorOperandKind.Boolean);

            //reserved 12 15//https://typekit.files.wordpress.com/2013/05/5176.cff.pdf
            //reserved 12 16//https://typekit.files.wordpress.com/2013/05/5176.cff.pdf

            Register(12, 17, "LanguageGroup", OperatorOperandKind.Number); //https://typekit.files.wordpress.com/2013/05/5176.cff.pdf
            Register(12, 18, "ExpansionFactor", OperatorOperandKind.Number); //https://typekit.files.wordpress.com/2013/05/5176.cff.pdf
            Register(12, 19, "initialRandomSeed", OperatorOperandKind.Number); //https://typekit.files.wordpress.com/2013/05/5176.cff.pdf

            Register(19, "Subrs", OperatorOperandKind.Number);
            Register(20, "defaultWidthX", OperatorOperandKind.Number);
            Register(21, "nominalWidthX", OperatorOperandKind.Number);
        }

        public static Operator GetOperatorByKey(byte b0, byte b1)
        {
            RegisteredOperators.TryGetValue((b1 << 8) | b0, out var found);
            return found;
        }

        internal static void Register(byte b0, byte b1, string operatorName, OperatorOperandKind opopKind)
        {
            RegisteredOperators.Add((b1 << 8) | b0, new Operator(operatorName, b0, b1, opopKind));
        }
        internal static void Register(byte b0, string operatorName, OperatorOperandKind opopKind)
        {
            RegisteredOperators.Add(b0, new Operator(operatorName, b0, 0, opopKind));
        }

        internal static readonly string[] StdStrings = [
            //Appendix A: Standard Strings
            ".notdef",
            "space",
            "exclam",
            "quotedbl",
            "numbersign",
            "dollar",
            "percent",
            "ampersand",
            "quoteright",
            "parenleft",
            "parenright",
            "asterisk",
            "plus",
            "comma",
            "hyphen",
            "period",
            "slash",
            "zero",
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
            "colon",
            "semicolon",
            "less",
            "equal",
            "greater",
            "question",
            "at",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "bracketleft",
            "backslash",
            "bracketright",
            "asciicircum",
            "underscore",
            "quoteleft",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "braceleft",
            "bar",
            "braceright",
            "asciitilde",
            "exclamdown",
            "cent",
            "sterling",
            "fraction",
            "yen",
            "florin",
            "section",
            "currency",
            "quotesingle",
            "quotedblleft",
            "guillemotleft",
            "guilsinglleft",
            "guilsinglright",
            "fi",
            "fl",
            "endash",
            "dagger",
            "daggerdbl",
            "periodcentered",
            "paragraph",
            "bullet",
            "quotesinglbase",
            "quotedblbase",
            "quotedblright",
            "guillemotright",
            "ellipsis",
            "perthousand",
            "questiondown",
            "grave",
            "acute",
            "circumflex",
            "tilde",
            "macron",
            "breve",
            "dotaccent",
            "dieresis",
            "ring",
            "cedilla",
            "hungarumlaut",
            "ogonek",
            "caron",
            "emdash",
            "AE",
            "ordfeminine",
            "Lslash",
            "Oslash",
            "OE",
            "ordmasculine",
            "ae",
            "dotlessi",
            "lslash",
            "oslash",
            "oe",
            "germandbls",
            "onesuperior",
            "logicalnot",
            "mu",
            "trademark",
            "Eth",
            "onehalf",
            "plusminus",
            "Thorn",
            "onequarter",
            "divide",
            "brokenbar",
            "degree",
            "thorn",
            "threequarters",
            "twosuperior",
            "registered",
            "minus",
            "eth",
            "multiply",
            "threesuperior",
            "copyright",
            "Aacute",
            "Acircumflex",
            "Adieresis",
            "Agrave",
            "Aring",
            "Atilde",
            "Ccedilla",
            "Eacute",
            "Ecircumflex",
            "Edieresis",
            "Egrave",
            "Iacute",
            "Icircumflex",
            "Idieresis",
            "Igrave",
            "Ntilde",
            "Oacute",
            "Ocircumflex",
            "Odieresis",
            "Ograve",
            "Otilde",
            "Scaron",
            "Uacute",
            "Ucircumflex",
            "Udieresis",
            "Ugrave",
            "Yacute",
            "Ydieresis",
            "Zcaron",
            "aacute",
            "acircumflex",
            "adieresis",
            "agrave",
            "aring",
            "atilde",
            "ccedilla",
            "eacute",
            "ecircumflex",
            "edieresis",
            "egrave",
            "iacute",
            "icircumflex",
            "idieresis",
            "igrave",
            "ntilde",
            "oacute",
            "ocircumflex",
            "odieresis",
            "ograve",
            "otilde",
            "scaron",
            "uacute",
            "ucircumflex",
            "udieresis",
            "ugrave",
            "yacute",
            "ydieresis",
            "zcaron",
            "exclamsmall",
            "Hungarumlautsmall",
            "dollaroldstyle",
            "dollarsuperior",
            "ampersandsmall",
            "Acutesmall",
            "parenleftsuperior",
            "parenrightsuperior",
            "twodotenleader",
            "onedotenleader",
            "zerooldstyle",
            "oneoldstyle",
            "twooldstyle",
            "threeoldstyle",
            "fouroldstyle",
            "fiveoldstyle",
            "sixoldstyle",
            "sevenoldstyle",
            "eightoldstyle",
            "nineoldstyle",
            "commasuperior",
            "threequartersemdash",
            "periodsuperior",
            "questionsmall",
            "asuperior",
            "bsuperior",
            "centsuperior",
            "dsuperior",
            "esuperior",
            "isuperior",
            "lsuperior",
            "msuperior",
            "nsuperior",
            "osuperior",
            "rsuperior",
            "ssuperior",
            "tsuperior",
            "ff",
            "ffi",
            "ffl",
            "parenleftinferior",
            "parenrightinferior",
            "Circumflexsmall",
            "hyphensuperior",
            "Gravesmall",
            "Asmall",
            "Bsmall",
            "Csmall",
            "Dsmall",
            "Esmall",
            "Fsmall",
            "Gsmall",
            "Hsmall",
            "Ismall",
            "Jsmall",
            "Ksmall",
            "Lsmall",
            "Msmall",
            "Nsmall",
            "Osmall",
            "Psmall",
            "Qsmall",
            "Rsmall",
            "Ssmall",
            "Tsmall",
            "Usmall",
            "Vsmall",
            "Wsmall",
            "Xsmall",
            "Ysmall",
            "Zsmall",
            "colonmonetary",
            "onefitted",
            "rupiah",
            "Tildesmall",
            "exclamdownsmall",
            "centoldstyle",
            "Lslashsmall",
            "Scaronsmall",
            "Zcaronsmall",
            "Dieresissmall",
            "Brevesmall",
            "Caronsmall",
            "Dotaccentsmall",
            "Macronsmall",
            "figuredash",
            "hypheninferior",
            "Ogoneksmall",
            "Ringsmall",
            "Cedillasmall",
            "questiondownsmall",
            "oneeighth",
            "threeeighths",
            "fiveeighths",
            "seveneighths",
            "onethird",
            "twothirds",
            "zerosuperior",
            "foursuperior",
            "fivesuperior",
            "sixsuperior",
            "sevensuperior",
            "eightsuperior",
            "ninesuperior",
            "zeroinferior",
            "oneinferior",
            "twoinferior",
            "threeinferior",
            "fourinferior",
            "fiveinferior",
            "sixinferior",
            "seveninferior",
            "eightinferior",
            "nineinferior",
            "centinferior",
            "dollarinferior",
            "periodinferior",
            "commainferior",
            "Agravesmall",
            "Aacutesmall",
            "Acircumflexsmall",
            "Atildesmall",
            "Adieresissmall",
            "Aringsmall",
            "AEsmall",
            "Ccedillasmall",
            "Egravesmall",
            "Eacutesmall",
            "Ecircumflexsmall",
            "Edieresissmall",
            "Igravesmall",
            "Iacutesmall",
            "Icircumflexsmall",
            "Idieresissmall",
            "Ethsmall",
            "Ntildesmall",
            "Ogravesmall",
            "Oacutesmall",
            "Ocircumflexsmall",
            "Otildesmall",
            "Odieresissmall",
            "OEsmall",
            "Oslashsmall",
            "Ugravesmall",
            "Uacutesmall",
            "Ucircumflexsmall",
            "Udieresissmall",
            "Yacutesmall",
            "Thornsmall",
            "Ydieresissmall",
            "001.000",
            "001.001",
            "001.002",
            "001.003",
            "Black",
            "Bold",
            "Book",
            "Light",
            "Medium",
            "Regular",
            "Roman",
            "Semibold"  
        ];//390

        internal static Dictionary<int, Operator> RegisteredOperators = [];

       


    }
}
