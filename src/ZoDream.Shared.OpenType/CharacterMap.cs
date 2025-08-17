using System.Collections.Generic;

namespace ZoDream.Shared.OpenType
{
    public abstract class CharacterMap
    {
        // https://www.microsoft.com/typography/otspec/cmap.htm
        public abstract ushort Format { get; }
        public ushort PlatformId { get; set; }
        public ushort EncodingId { get; set; }
    }

    public class NullCharMap : CharacterMap
    {
        public override ushort Format => 0;
    }

    public class CharMapFormat4 : CharacterMap
    {
        public override ushort Format => 4;

        internal readonly ushort[] StartCode; //Starting character code for each segment
        internal readonly ushort[] EndCode;//Ending character code for each segment, last = 0xFFFF.      
        internal readonly ushort[] IdDelta; //Delta for all character codes in segment
        internal readonly ushort[] IdRangeOffset; //Offset in bytes to glyph indexArray, or 0 (not offset in bytes unit)
        internal readonly ushort[] GlyphIdArray;
        public CharMapFormat4(ushort[] startCode, ushort[] endCode, ushort[] idDelta, ushort[] idRangeOffset, ushort[] glyphIdArray)
        {
            StartCode = startCode;
            EndCode = endCode;
            IdDelta = idDelta;
            IdRangeOffset = idRangeOffset;
            GlyphIdArray = glyphIdArray;
        }
    }

    public class CharMapFormat6 : CharacterMap
    {
        public override ushort Format => 6;
        internal readonly ushort StartCode;
        internal readonly ushort[] GlyphIdArray;

        internal CharMapFormat6(ushort startCode, ushort[] glyphIdArray)
        {
            GlyphIdArray = glyphIdArray;
            StartCode = startCode;
        }
    }

    public class CharMapFormat12 : CharacterMap
    {
        public override ushort Format => 12;

        uint[] StartCharCodes, EndCharCodes, StartGlyphIds;
        internal CharMapFormat12(uint[] startCharCodes, uint[] endCharCodes, uint[] startGlyphIds)
        {
            StartCharCodes = startCharCodes;
            EndCharCodes = endCharCodes;
            StartGlyphIds = startGlyphIds;
        }
    }

    public class CharMapFormat14 : CharacterMap
    {
        public override ushort Format => 14;

        public Dictionary<int, VariationSelector> VariationSelectors { get; internal set; }
    }

    public class VariationSelector
    {
        public List<int> DefaultStartCodes = [];
        public List<int> DefaultEndCodes = [];
        public Dictionary<int, ushort> UVSMappings = [];
    }
}
