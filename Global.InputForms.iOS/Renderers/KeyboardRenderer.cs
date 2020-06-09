using System;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CollectionKeyboard), typeof(KeyboardRenderer))]
namespace Global.InputForms.iOS.Renderers
{
    public class KeyboardRenderer : ViewRenderer
    {
        public KeyboardRenderer()
        {
        }
    }
}
