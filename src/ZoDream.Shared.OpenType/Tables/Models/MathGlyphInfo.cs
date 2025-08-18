namespace ZoDream.Shared.OpenType.Tables
{
    public class MathGlyphInfo
    {
        public readonly ushort GlyphIndex;
        public MathGlyphInfo(ushort glyphIndex)
        {
            this.GlyphIndex = glyphIndex;
        }

        public MathValueRecord? ItalicCorrection { get; internal set; }
        public MathValueRecord? TopAccentAttachment { get; internal set; }
        public bool IsShapeExtensible { get; internal set; }
        public bool HasSomeMathKern { get; private set; }

        //
        MathKernInfoRecord _mathKernRec;
        internal void SetMathKerns(MathKernInfoRecord mathKernRec)
        {
            _mathKernRec = mathKernRec;
            HasSomeMathKern = true;
        }
        /// <summary>
        /// vertical glyph construction
        /// </summary>
        public MathGlyphConstruction VertGlyphConstruction;
        /// <summary>
        /// horizontal glyph construction
        /// </summary>
        public MathGlyphConstruction HoriGlyphConstruction;
    }

}
