using ZoDream.Shared.OpenType.CompactFontFormat;

namespace ZoDream.Shared.OpenType.Tables
{
    public class GlyphData
    {
        public static readonly GlyphData Empty = new([], [], GlyphBound.Zero, null, 0);
        public ushort GlyphIndex { get; }                       //FOUND in all mode
        public GlyphBound Bounds { get; set; }             //FOUND in all mode
        public ushort OriginalAdvanceWidth { get; set; } //FOUND in all mode
        internal ushort BitmapGlyphAdvanceWidth { get; set; }    //FOUND in all mode

        //TrueTypeFont
        public ushort[] EndPoints { get; set; }         //NULL in  _onlyLayoutEssMode         
        public GlyphPoint[] GlyphPoints { get; set; }  //NULL in  _onlyLayoutEssMode         
        public byte[] GlyphInstructions { get; set; }         //NULL in _onlyLayoutEssMode 
        public bool HasGlyphInstructions => GlyphInstructions != null; //FALSE  n _onlyLayoutEssMode 

        //
        public GlyphClassKind GlyphClass { get; internal set; } //FOUND in all mode
        internal ushort MarkClassDef { get; set; }              //FOUND in all mode
        internal readonly FontGlyphData Glyph;

        internal uint BitmapStreamOffset { get; private set; }
        internal uint BitmapFormat { get; set; }
        public uint StreamLen;            //FOUND in all mode (if font has this data)
        public ushort ImgFormat;          //FOUND in all mode (if font has this data)

        public GlyphData(
            GlyphPoint[] glyphPoints,
            ushort[] contourEndPoints,
            GlyphBound bounds,
            byte[] glyphInstructions,
            ushort index)
        {
            GlyphPoints = glyphPoints;
            EndPoints = contourEndPoints;
            Bounds = bounds;
            GlyphInstructions = glyphInstructions;
            GlyphIndex = index;

        }

        public GlyphData(FontGlyphData cff1Glyph, ushort glyphIndex)
        {
            Glyph = cff1Glyph;
            GlyphIndex = glyphIndex;
        }


        public GlyphData(ushort glyphIndex, uint streamOffset, uint streamLen, ushort imgFormat)
        {
            //_bmpGlyphSource = bmpGlyphSource;
            BitmapStreamOffset = streamOffset;
            StreamLen = streamLen;
            ImgFormat = imgFormat;
            GlyphIndex = glyphIndex;
        }
    }
}
