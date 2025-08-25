using System;

namespace ZoDream.Shared.Font
{
    public class FontMeta
    {
        public Version Version { get; set; } = new Version(1, 0, 0);

        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public string FamilyName { get; set; } = string.Empty;

        public FontWeightName WeightName { get; set; }

        public FontSerifStyle SerifStyle { get; set; }

        public FontSlantStyle SlantStyle { get; set; }

        public FontWidthStyle WidthStyle { get; set; }

        public string Manufacturer {  get; set; } = string.Empty;

        public string Designer { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Copyright { get; set; } = string.Empty;

        public string LicenseDescription { get; set; } = string.Empty;
        public string LicenseUrl { get; set; } = string.Empty;

        public string VendorUrl { get; set; } = string.Empty;
        public string DesignerUrl { get; set; } = string.Empty;

        public string SampleText { get; set; } = string.Empty;
    }
}