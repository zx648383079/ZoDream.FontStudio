using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public abstract class GlyphShapingTable : ITypefaceTable
    {
        public abstract string Name { get; }

        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }

        public ScriptList ScriptList { get; set; }
        public FeatureList FeatureList { get; set; }

        /// <summary>
        /// read script_list, feature_list, and skip look up table
        /// </summary>
        internal bool OnlyScriptList { get; set; } //
    }
}
