using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ZoDream.FontStudio.ViewModels;

namespace ZoDream.FontStudio.Controls
{
    public class GlyphItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? GlyphTemplate { get; set; }
        public DataTemplate? GroupTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is GlyphGroupViewModel)
            {
                return GroupTemplate;
            }
            if (GlyphTemplate is null)
            {
                return base.SelectTemplateCore(item);
            }
            return GlyphTemplate;
        }
    }
}
