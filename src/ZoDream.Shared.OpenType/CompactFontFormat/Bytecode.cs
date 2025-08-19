namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class Bytecode
    {

        public FontSet FontSet { get; set; } = new();

        public FontFamily CurrentFont {  get; set; }

        public DataDicEntry[] TopDic {  get; set; }
        public FontInfo CidFontInfo { get; internal set; }
        public int PrivateDICTLen { get; internal set; }
        public int PrivateDICTOffset { get; internal set; }
        public bool UseCompactInstruction { get; internal set; }
        public Type2InstructionCompacter InstCompacter { get; internal set; }

        public long CffStartAt;

        public int CharStringsOffset;
        public int CharsetOffset;
        public int EncodingOffset;
    }
}
