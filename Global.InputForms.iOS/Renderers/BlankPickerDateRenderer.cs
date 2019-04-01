using System;
using System.ComponentModel;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankPickerDateRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            SetAttributes();

            if (Element is BlankDatePicker picker)
                Control.ShouldEndEditing += textField =>
                {
                    var seletedDate = textField;
                    var text = seletedDate.Text;
                    if (text == picker.Placeholder) Control.Text = DateTime.Now.ToString(picker.Format);
                    return true;
                };
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetAttributes();
        }

        private void SetAttributes()
        {
            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                if (Element is BlankDatePicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                    Control.Text = picker.Placeholder;
            }
        }
    }
}