﻿namespace ZoDream.Shared.OpenType.Tables
{
    public interface IGlyphIndexList
    {
        int Count { get; }
        ushort this[int index] { get; }

        /// <summary>
        /// remove:add_new 1:1
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newGlyphIndex"></param>
        void Replace(int index, ushort newGlyphIndex);
        /// <summary>
        /// remove:add_new >=1:1
        /// </summary>
        /// <param name="index"></param>
        /// <param name="removeLen"></param>
        /// <param name="newGlyphIndex"></param>
        void Replace(int index, int removeLen, ushort newGlyphIndex);
        /// <summary>
        /// remove: add_new 1:>=1
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newGlyphIndices"></param>
        void Replace(int index, ushort[] newGlyphIndices);
    }
}