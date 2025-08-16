using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.WebType
{
    public partial class WOFF2Reader
    {
        static readonly string[] _knownTableTags = new string[]
        {
             //Known Table Tags
            //Flag  Tag         Flag  Tag       Flag  Tag        Flag    Tag
            //0	 => cmap,	    16 =>EBLC,	    32 =>CBDT,	     48 =>gvar,
            //1  => head,	    17 =>gasp,	    33 =>CBLC,	     49 =>hsty,
            //2	 => hhea,	    18 =>hdmx,	    34 =>COLR,	     50 =>just,
            //3	 => hmtx,	    19 =>kern,	    35 =>CPAL,	     51 =>lcar,
            //4	 => maxp,	    20 =>LTSH,	    36 =>SVG ,	     52 =>mort,
            //5	 => name,	    21 =>PCLT,	    37 =>sbix,	     53 =>morx,
            //6	 => OS/2,	    22 =>VDMX,	    38 =>acnt,	     54 =>opbd,
            //7	 => post,	    23 =>vhea,	    39 =>avar,	     55 =>prop,
            //8	 => cvt ,	    24 =>vmtx,	    40 =>bdat,	     56 =>trak,
            //9	 => fpgm,	    25 =>BASE,	    41 =>bloc,	     57 =>Zapf,
            //10 =>	glyf,	    26 =>GDEF,	    42 =>bsln,	     58 =>Silf,
            //11 =>	loca,	    27 =>GPOS,	    43 =>cvar,	     59 =>Glat,
            //12 =>	prep,	    28 =>GSUB,	    44 =>fdsc,	     60 =>Gloc,
            //13 =>	CFF ,	    29 =>EBSC,	    45 =>feat,	     61 =>Feat,
            //14 =>	VORG,	    30 =>JSTF,	    46 =>fmtx,	     62 =>Sill,
            //15 =>	EBDT,	    31 =>MATH,	    47 =>fvar,	     63 =>arbitrary tag follows,...
            //-------------------------------------------------------------------

            //-- TODO:implement missing table too!
            CmapTable.TableName, //0
            HeadTable.TableName, //1
            HorizontalHeaderTable.TableName,//2
            HorizontalMetricsTable.TableName,//3
            MaxProfileTable.TableName,//4
            NameTable.TableName,//5
            OS2WindowsMetricsTable.TableName, //6
            PostTable.TableName,//7
            ControlValueTable.TableName,//8
            FontProgramTable.TableName,//9
            GlyphDataTable.TableName,//10
            GlyphLocationsTable.TableName,//11
            ControlValueProgramTable.TableName,//12
            CompactFontFormatTable.TableName,//13
            VerticalOriginTable.TableName,//14 
            EmbeddedBitmapDataTable.TableName,//15, 

            
            //---------------
            EmbeddedBitmapLocationTable.TableName,//16
            GridFittingScanConversionProcedureTable.TableName,//17
            HorizontalDeviceMetricsTable.TableName,//18
            KernTable.TableName,//19
            LinearThresholdTable.TableName,//20 
            Pcl5Table.TableName,//21
            VerticalDeviceMetricsTable.TableName,//22
            VerticalHeaderTable.TableName,//23
            VerticalMetricsTable.TableName,//24
            BaseTable.TableName,//25
            GlyphDefinitionTable.TableName,//26
            GlyphPositioningTable.TableName,//27
            GlyphSubstitutionTable.TableName,//28            
            EmbeddedBitmapScalingTable.TableName, //29
            JustificationTable.TableName, //30
            MathTable.TableName,//31
             //---------------


            //Known Table Tags (copy,same as above)
            //Flag  Tag         Flag  Tag       Flag  Tag        Flag    Tag
            //0	 => cmap,	    16 =>EBLC,	    32 =>CBDT,	     48 =>gvar,
            //1  => head,	    17 =>gasp,	    33 =>CBLC,	     49 =>hsty,
            //2	 => hhea,	    18 =>hdmx,	    34 =>COLR,	     50 =>just,
            //3	 => hmtx,	    19 =>kern,	    35 =>CPAL,	     51 =>lcar,
            //4	 => maxp,	    20 =>LTSH,	    36 =>SVG ,	     52 =>mort,
            //5	 => name,	    21 =>PCLT,	    37 =>sbix,	     53 =>morx,
            //6	 => OS/2,	    22 =>VDMX,	    38 =>acnt,	     54 =>opbd,
            //7	 => post,	    23 =>vhea,	    39 =>avar,	     55 =>prop,
            //8	 => cvt ,	    24 =>vmtx,	    40 =>bdat,	     56 =>trak,
            //9	 => fpgm,	    25 =>BASE,	    41 =>bloc,	     57 =>Zapf,
            //10 =>	glyf,	    26 =>GDEF,	    42 =>bsln,	     58 =>Silf,
            //11 =>	loca,	    27 =>GPOS,	    43 =>cvar,	     59 =>Glat,
            //12 =>	prep,	    28 =>GSUB,	    44 =>fdsc,	     60 =>Gloc,
            //13 =>	CFF ,	    29 =>EBSC,	    45 =>feat,	     61 =>Feat,
            //14 =>	VORG,	    30 =>JSTF,	    46 =>fmtx,	     62 =>Sill,
            //15 =>	EBDT,	    31 =>MATH,	    47 =>fvar,	     63 =>arbitrary tag follows,...
            //-------------------------------------------------------------------

            ColorBitmapDataTable.TableName, //32
            ColorBitmapLocationTable.TableName,//33
            ColorTable.TableName,//34
            ColorPaletteTable.TableName,//35,
            SvgTable.TableName,//36
            StandardBitmapGraphicsTable.TableName,//37
            "acnt",//38
            AxisVariationsTable.TableName,//39
            "bdat",//40
            "bloc",//41
            "bsln",//42
            CVTVariationsTable.TableName,//43
            "fdsc",//44
            "feat",//45
            "fmtx",//46
            FontVariationsTable.TableName,//47
             //---------------

            GlyphVariationsTable.TableName,//48
            "hsty",//49
            "just",//50
            "lcar",//51
            "mort",//52
            "morx",//53
            "opbd",//54
            "prop",//55
            "trak",//56
            "Zapf",//57
            "Silf",//58
            "Glat",//59
            "Gloc",//60
            "Feat",//61
            "Sill",//62
            "...." //63 arbitrary tag follows
        };
    }
}
