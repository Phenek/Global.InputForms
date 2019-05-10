using System;
using System.ComponentModel;
using Android.Content;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(BlankPicker), typeof(BlankPickerRenderer))]
namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerRenderer : PickerRenderer
    {
        public BlankPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            SetAttributes();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetAttributes();
        }


        private void SetAttributes()
        {
            Control.SetBackgroundColor(Color.Transparent);

            if (Element is BlankPicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                Control.Text = picker.Placeholder;
        }
    }
}