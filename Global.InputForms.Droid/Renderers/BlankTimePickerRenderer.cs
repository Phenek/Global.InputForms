using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Widget;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(BlankTimePicker), typeof(BlankPickerRenderer))]
namespace Global.InputForms.Droid.Renderers
{
    public class BlankTimePickerRenderer : TimePickerRenderer
    {
        TimePickerDialog _dialog;
        IElementController EController => Element as IElementController;
        BlankTimePicker blankPicker;

        public BlankTimePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankTimePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    //this.Control.Click += OnPickerClick;
                    //this.Control.Text = Element.Time.ToString(Element.Format);
                    //this.Control.KeyListener = null;

                    Control.TextChanged += (sender, arg) =>
                    {
                        var selectedDate = arg.Text.ToString();
                        if (selectedDate == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);
                    };
                }
            }
            SetPlaceholder();
            SetAlignment();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetPlaceholder();

            if (e.PropertyName == nameof(BlankPicker.HorizontalTextAlignment)) SetAlignment();
        }


        private void SetPlaceholder()
        {
            Control.SetBackgroundColor(Color.Transparent);

            if (Element is BlankTimePicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                Control.Text = picker.Placeholder;
        }

        private void SetAlignment()
        {
            switch (((BlankTimePicker)Element).HorizontalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.TextAlignment = Android.Views.TextAlignment.Center;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.TextAlignment = Android.Views.TextAlignment.ViewEnd;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.TextAlignment = Android.Views.TextAlignment.ViewStart;
                    break;
            }
        }


        public void OnPickerClick(object sender, EventArgs e)
        {
            _dialog = new TimePickerDialog(Context, (s, d) =>
            {
                EController.SetValueFromRenderer(Xamarin.Forms.TimePicker.TimeProperty, new TimeSpan(d.HourOfDay, d.Minute, 0));
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();

                _dialog = null;
            }, this.Element.Time.Hours, this.Element.Time.Minutes, true);

            _dialog.SetButton(blankPicker.CancelButtonText, (s, el) =>
            {
                blankPicker.SendCancelClicked();
            });
            _dialog.SetButton2(blankPicker.DoneButtonText, (k, p) =>
            {
                this.Control.Text = Element.Time.ToString(Element.Format);
                blankPicker.SendDoneClicked();
            });
            _dialog.Show();
        }
    }
}