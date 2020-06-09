using System.ComponentModel;
using CoreGraphics;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TestEntry), typeof(TestEntryRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class TestEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            SetAttributes();

            if (Element is TestEntry entry)
            {
                var rect = new CGRect(x: 0, y: 0, width: 0, height: 200);
                var nativKeyboard = ConvertFormsToNative(entry.KeyboardInput, rect);

                UIView KeyboardView = new UIView(rect);
                KeyboardView.BackgroundColor = UIColor.Red;
                KeyboardView.AddSubview(nativKeyboard);
                Control.InputView = KeyboardView;
            }
        }

        internal static UIView ConvertFormsToNative(View view, CGRect size)
        {
            var renderer = Platform.CreateRenderer(view);

            renderer.NativeView.Frame = size;

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            renderer.Element.Layout(size.ToRectangle());

            var nativeView = renderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;

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