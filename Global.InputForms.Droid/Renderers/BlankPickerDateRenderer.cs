using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Views;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using TextAlignment = Android.Views.TextAlignment;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerDateRenderer : DatePickerRenderer
    {
        private DatePickerDialog _dialog;
        private BlankDatePicker blankPicker;

        public BlankPickerDateRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
                if (Control != null)
                {
                    Control.Click += OnPickerClick;
                    Control.Text = Element.Date.ToString(Element.Format);
                    Control.KeyListener = null;

                    Control.TextChanged += (sender, arg) =>
                    {
                        var selectedDate = arg.Text.ToString();
                        if (selectedDate == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);
                    };
                }

            if (e.OldElement != null) Control.Click -= OnPickerClick;
            SetPlaceholder();
            SetAlignment();
            Control.SetPadding(0, 7, 0, 3);
            Control.Gravity = GravityFlags.Fill;
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
            switch (((BlankDatePicker) Element).HorizontalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.TextAlignment = TextAlignment.Center;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.TextAlignment = TextAlignment.ViewEnd;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.TextAlignment = TextAlignment.ViewStart;
                    break;
            }
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public void OnPickerClick(object sender, EventArgs e)
        {
            var model = Element;
            _dialog = new DatePickerDialog(Context, (o, d) =>
                {
                    model.Date = d.Date;
                    EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    Control.ClearFocus();

                    _dialog = null;
                }, Element.Date.Year, Element.Date.Month - 1, Element.Date.Day);
            _dialog.DatePicker.MaxDate = UnixTimestampFromDateTime(Element.MaximumDate);
            _dialog.DatePicker.MinDate = UnixTimestampFromDateTime(Element.MinimumDate);

            _dialog.SetButton(blankPicker.DoneButtonText, (k, p) =>
            {
                Control.Text = _dialog.DatePicker.DateTime.ToString(Element.Format);
                Element.Date = _dialog.DatePicker.DateTime;
                blankPicker.SendDoneClicked();
            });
            _dialog.SetButton2(blankPicker.CancelButtonText, (s, el) => { blankPicker.SendCancelClicked(); });
            _dialog.Show();
        }
    }
}