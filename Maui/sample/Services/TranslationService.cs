using System;
using System.Resources;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Sample.ResX;

namespace Sample.Services
{
    public static class Translate
    {
        private static ResourceManager resourceMan;

        internal static ResourceManager ResourceManager
        {
            get
            {
                if (object.Equals(null, resourceMan))
                {
                    ResourceManager temp = new ResourceManager("Sample.ResX.AppResources", typeof(AppResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        public static string GetText(string text)
        {
            return ResourceManager.GetString(text, Resources.Culture);
        }

        public static string GetPlural(string text, int plural = 0)
        {
            if (plural > 1)
            {
                var str = GetText(text + ".Plural");
                if (str == null) str = GetText(text);
                return str != null ? string.Format(str, plural) : string.Empty;
            }
            var single = GetText(text);
            return single != null ? string.Format(single, plural) : string.Empty;
        }
    }

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {

        internal static TranslateExtension Preserve()
        {
            return new TranslateExtension(false);
        }

        public TranslateExtension() : this(true) { }

        internal TranslateExtension(bool shouldInit)
        {
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Translate.GetText(Text);
        }
    }
}

