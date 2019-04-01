using System;
using System.ComponentModel;
using Android.Content;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerDateRenderer : DatePickerRenderer
    {
        public BlankPickerDateRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            SetAttributes();

            if (Element is BlankDatePicker picker)
                Control.TextChanged += (sender, arg) =>
                {
                    var selectedDate = arg.Text.ToString();
                    if (selectedDate == picker.Placeholder) Control.Text = DateTime.Now.ToString(picker.Format);
                };
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetAttributes();
        }


        private void SetAttributes()
        {
            Control.SetBackgroundColor(Color.Transparent);

            if (Element is BlankDatePicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                Control.Text = picker.Placeholder;
        }
    }
}