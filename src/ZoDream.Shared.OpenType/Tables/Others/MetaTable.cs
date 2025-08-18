using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class MetaTable : ITypefaceTable
    {
        public const string TableName = "Meta";

        public string Name => TableName;

        /// <summary>
        /// dlng tags
        /// </summary>
        public string[] DesignLanguageTags { get; set; }
        /// <summary>
        /// slng tags
        /// </summary>
        public string[] SupportedLanguageTags { get; set; }
    }
}
