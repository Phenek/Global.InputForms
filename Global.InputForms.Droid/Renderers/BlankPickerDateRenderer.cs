using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Widget;
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
        DatePickerDialog _dialog;
        IElementController EController => Element as IElementController;
        BlankDatePicker blankPicker;

        public BlankPickerDateRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    this.Control.Click += OnPickerClick;
                    this.Control.Text = Element.Date.ToString(Element.Format);
                    this.Control.KeyListener = null;

                    Control.TextChanged += (sender, arg) =>
                    {
                        var selectedDate = arg.Text.ToString();
                        if (selectedDate == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);
                    };
                }
            }
            if (e.OldElement != null)
            {
                this.Control.Click -= OnPickerClick;
            }
            SetPlaceholder();
            SetAlignment();
            Control.SetPadding(0, 7, 0, 3);
            Control.Gravity = Android.Views.GravityFlags.Fill;
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

            if (Element is BlankDatePicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                Control.Text = picker.Placeholder;
        }

        private void SetAlignment()
        {
            switch (((BlankDatePicker)Element).HorizontalTextAlignment)
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

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public void OnPickerClick(object sender, EventArgs e)
        {
            Xamarin.Forms.DatePicker model = Element;
            _dialog = new DatePickerDialog(Context, (o, d) =>
            {
                model.Date = d.Date;
                EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            _dialog.DatePicker.MaxDate = UnixTimestampFromDateTime(Element.MaximumDate);
            _dialog.DatePicker.MinDate = UnixTimestampFromDateTime(Element.MinimumDate);

            _dialog.SetButton(blankPicker.DoneButtonText, (k, p) =>
            {
                this.Control.Text = _dialog.DatePicker.DateTime.ToString(Element.Format);
                Element.Date = _dialog.DatePicker.DateTime;
                blankPicker.SendDoneClicked();
            });
            _dialog.SetButton2(blankPicker.CancelButtonText, (s, el) =>
            {
                blankPicker.SendCancelClicked();
            });
            _dialog.Show();
        }
    }
}