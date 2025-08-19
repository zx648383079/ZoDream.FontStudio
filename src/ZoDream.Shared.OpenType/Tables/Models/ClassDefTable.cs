using System;

namespace ZoDream.Shared.OpenType.Tables
{
    public class ClassDefTable
    {
        public int Format { get; internal set; }
        //----------------
        //format 1
        public ushort startGlyph;
        public ushort[] classValueArray;
        //---------------
        //format2
        public ClassRangeRecord[] records;

        public int GetClassValue(ushort glyphIndex)
        {
            switch (Format)
            {
                default: throw new NotSupportedException();
                case 1:
                    {
                        if (glyphIndex >= startGlyph &&
                            glyphIndex < classValueArray.Length)
                        {
                            return classValueArray[glyphIndex - startGlyph];
                        }
                        return -1;
                    }
                case 2:
                    {

                        for (int i = 0; i < records.Length; ++i)
                        {
                            //TODO: review a proper method here again
                            //esp. binary search
                            ClassRangeRecord rec = records[i];
                            if (rec.startGlyphId <= glyphIndex)
                            {
                                if (glyphIndex <= rec.endGlyphId)
                                {
                                    return rec.classNo;
                                }
                            }
                            else
                            {
                                return -1;//no need to go further
                            }
                        }
                        return -1;
                    }
            }
        }
    }
}
