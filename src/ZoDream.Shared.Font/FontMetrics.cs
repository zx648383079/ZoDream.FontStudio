namespace ZoDream.Shared.Font
{
    public class FontMetrics
    {
        /// <summary>
        /// The granularity of the coordinate grid.
        /// </summary>
        public int Granularity { get; set; }
        /// <summary>
        /// The point above which clipping can safely occur.
        /// </summary>
        public int ClippingAscender { get; set; }
        /// <summary>
        /// The typographical ascender relative to the baseline.
        /// </summary>
        public int Ascender { get; set; }
        /// <summary>
        /// The cap height relative to the baseline.
        /// </summary>
        public int CapHeight { get; set; }
        /// <summary>
        /// The x-height relative to the baseline.
        /// </summary>
        public int XHeight { get; set; }
        /// <summary>
        /// The baseline.
        /// </summary>
        public int Baseline { get; set; }
        /// <summary>
        /// The typographical descender relative to the baseline.
        /// </summary>
        public int Descender { get; set; }
        /// <summary>
        /// The point below which clipping can safely occur.
        /// </summary>
        public int ClippingDescender { get; set; }
        /// <summary>
        /// The typographical line gap.
        /// </summary>
        public int LineGap { get; set; }
    }
}
