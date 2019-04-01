using System.ComponentModel;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankEntry), typeof(BlankEntryRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            SetAttributes();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.IsEnabled)) SetAttributes();
        }

        private void SetAttributes()
        {
            if (Control != null) Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}